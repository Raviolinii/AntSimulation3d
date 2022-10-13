using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntsMaster : MonoBehaviour
{

    protected Anthill anthill;
    protected Owner owner;


    // Food
    public int maxFoodAmount = 500;
    public int foodGathered;
    //int maxReachableFoodAmount = 2_000;
    protected int foodIncreaseValue = 200;


    // Population
    protected int population = 0;
    protected int maxPopulation = 60;
    //protected int maxReachablePopulation = 300;
    protected int populationIncreaseValue = 30;
    // was 10 start and 5 increase
    // good was 40 and 20 increase

    protected int spawnMultiply = 5;

    // Ants
    public GameObject antWorkerPrefab;
    public GameObject antWarriorPrefab;
    protected List<WorkerAnt> antWorkers = new List<WorkerAnt>();
    protected List<AntWarrior> antWarriors = new List<AntWarrior>();
    protected int supplyAnts = 0;
    public int movementRange = 50;
    public int warriorsStacked = 0;


    // Coroutine Ants
    Coroutine workerSpawnCoroutine;
    Coroutine warriorSpawnCoroutine;
    Coroutine supplyAntSpawnCoroutine;
    float workerSpawnDelay = 1.5f;
    float warriorSpawnDelay = 1.5f;
    float supplyAntsSpawnDelay = 2.5f;
    protected int workersQueued = 0;
    protected int warriorsQueued = 0;
    protected bool supplyAntQueued = false;


    // Buy Ants
    protected int workerPrice = 60;
    protected int warriorPrice = 100;
    protected int supplyAntPrice = 200;


    // Alarm
    public bool dangerSpotted = false;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        Invoke("FindAnthill", 2f);
        Invoke("SetAnthillMaster", 2.2f);

        //Invoke("SpawnWorker", 2.5f);
        //Invoke("SpawnWarrior", 2.5f);

        Invoke("SetInitialFoodAmount", 2.5f);
        //AddFood(500);

        /* Invoke("BuyWorker", 2.5f);
        Invoke("BuyWorker", 2.5f);
        Invoke("BuyWorker", 2.5f);

        Invoke("BuyWarrior", 2.5f);
        Invoke("BuyWarrior", 4.5f);
        Invoke("BuyWarrior", 6.5f); */
        //Invoke("Zerg", 2.5f);

        //Invoke("BuySupplyAnt", 2.5f);
        //Invoke("BuySupplyAnt", 5.5f);
        //Invoke("SupplyAntDied", 10f);
    }

    void Zerg()
    {
        for (int i = 0; i < 70; i++)
        {
            SpawnWorker();
        }
        for (int i = 0; i < 30; i++)
        {
            SpawnWarrior();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    // Buy Ants
    public virtual void BuyWorker()
    {
        if (CanAddAnt(spawnMultiply) && foodGathered >= workerPrice)
        {
            SpendFood(workerPrice);
            for(int i = 0; i < spawnMultiply; i++)
                workersQueued++;

            if (workerSpawnCoroutine == null)
            {
                workerSpawnCoroutine = StartCoroutine(WorkerSpawnIEnumerator());
            }
        }
    }

    public virtual void BuyWarrior()
    {
        if (CanAddAnt(spawnMultiply) && foodGathered >= warriorPrice)
        {
            SpendFood(warriorPrice);
            for(int i = 0; i < spawnMultiply; i++)
                warriorsQueued++;

            if (dangerSpotted)
            {
                if (warriorSpawnCoroutine == null && warriorsQueued > 0)
                {
                    warriorSpawnCoroutine = StartCoroutine(WarriorSpawnIEnumerator());
                }
            }
        }
    }

    public virtual void BuySupplyAnt()
    {
        if (CanIncreasePopulation() && !supplyAntQueued && foodGathered >= supplyAntPrice)
        {
            SpendFood(supplyAntPrice);
            supplyAntQueued = true;
            supplyAntSpawnCoroutine = StartCoroutine(SupplyAntSpawnIEnumerator());
        }
    }

    // Alarm
    public void Alarm()
    {
        dangerSpotted = true;
        if (warriorsQueued > 0 && warriorSpawnCoroutine == null)
            warriorSpawnCoroutine = StartCoroutine(WarriorSpawnIEnumerator());
    }


    // Coroutine Ants
    protected virtual IEnumerator WorkerSpawnIEnumerator()
    {
        yield return new WaitForSeconds(workerSpawnDelay);

        SpawnWorker();
        workersQueued--;

        if (workersQueued > 0)
            workerSpawnCoroutine = StartCoroutine(WorkerSpawnIEnumerator());

        else
            workerSpawnCoroutine = null;
    }

    protected virtual IEnumerator WarriorSpawnIEnumerator()
    {
        yield return new WaitForSeconds(warriorSpawnDelay);

        SpawnWarrior();
        warriorsQueued--;

        if (warriorsQueued > 0)
            warriorSpawnCoroutine = StartCoroutine(WarriorSpawnIEnumerator());
        else
            warriorSpawnCoroutine = null;
    }

    protected virtual IEnumerator SupplyAntSpawnIEnumerator()
    {
        yield return new WaitForSeconds(supplyAntsSpawnDelay);
        SpawnSupplyAnt();
        supplyAntSpawnCoroutine = null;
    }


    // Ants
    Vector3 AsignYPosition(Vector3 position)
    {
        position.y = Terrain.activeTerrain.SampleHeight(position);
        return position;
    }

    protected virtual void SpawnWorker()
    {
        if (CanAddAnt())
        {
            Tile[] surroundings = anthill.GetSurroundings();
            int maxFoodPheromone = -1;
            int index = -1;
            for (int i = 0; i < 16; i++)
            {
                if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.no)
                {
                    int pheromoneValue = surroundings[i].GetWorkerFoodPheromoneValue();
                    if (maxFoodPheromone < pheromoneValue)
                    {
                        maxFoodPheromone = pheromoneValue;
                        index = i;
                    }
                }
            }

            Vector3 position = surroundings[index].transform.position;
            position = AsignYPosition(position);

            GameObject newAnt = Instantiate(antWorkerPrefab, position, antWorkerPrefab.transform.rotation);
            WorkerAnt newAntScript = newAnt.GetComponent<WorkerAnt>();
            newAntScript.SetOwner(owner);
            newAntScript.SetAnthillsPosition(anthill.transform.position);
            newAntScript.SetMovementRange(movementRange);
            newAntScript.SetMaster(this);
            antWorkers.Add(newAntScript);
            IncreasePopulation();
        }
    }

    protected virtual void SpawnWarrior()
    {
        if (CanAddAnt())
        {
            Tile[] surroundings = anthill.GetSurroundings();
            int maxWarriorPheromone = -1;
            int index = -1;
            for (int i = 0; i < 16; i++)
            {
                if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.no)
                {
                    int pheromoneValue = surroundings[i].GetWarriorPheromoneValue();
                    if (maxWarriorPheromone < pheromoneValue)
                    {
                        maxWarriorPheromone = pheromoneValue;
                        index = i;
                    }
                }
            }
            Vector3 position = surroundings[index].transform.position;
            position = AsignYPosition(position);
            GameObject newAnt = Instantiate(antWarriorPrefab, position, antWarriorPrefab.transform.rotation);
            AntWarrior newAntScript = newAnt.GetComponent<AntWarrior>();
            newAntScript.SetOwner(owner);
            newAntScript.SetMaster(this);
            antWarriors.Add(newAntScript);
            IncreasePopulation();

        }
    }

    protected void SpawnSupplyAnt()
    {
        anthill.CreateSupplyAnt();
        IncreaseMaxPopulation();
        IncreseMaxFoodAmount();
        supplyAntQueued = false;
        supplyAnts++;
    }

    public void SetMovementRange(int value)
    {
        movementRange = value;
        for (int i = 0; i < antWorkers.Count; i++)
        {
            antWorkers[i].SetMovementRange(value);
        }
    }

    public void SupplyAntDied()
    {
        DecreaseMaxPopulation();
        DecreaseMaxFoodAmount();
        supplyAnts--;
    }

    public int GetWorkersQueued() => workersQueued;
    public int GetWarriorsQueued() => warriorsQueued;
    public bool ISSupplyAntQueued() => supplyAntQueued;


    // Population
    public int GetPopulation() => population;
    public virtual void IncreasePopulation(int value) => population += value;
    public virtual void IncreasePopulation() => population++;
    public virtual void DecreasePopulation(int value) => population -= value;
    public virtual void DecreasePopulation() => population--;
    public bool CanAddAnt() => population < maxPopulation ? true : false;
    public bool CanAddAnt(int x) => warriorsQueued + workersQueued + population <= maxPopulation - x ? true : false;
    public virtual void IncreaseMaxPopulation() => maxPopulation += populationIncreaseValue;
    public virtual void DecreaseMaxPopulation() => maxPopulation -= populationIncreaseValue;
    public bool CanIncreasePopulation() => supplyAnts < 8 ? true : false;


    // Initialization
    protected virtual void SetInitialFoodAmount() => foodGathered = maxFoodAmount;
    protected void SetAnthillMaster() => anthill.SetMaster(this);
    protected void FindAnthill()
    {
        var anthillsFound = FindObjectsOfType<Anthill>();
        for (int i = 0; i < anthillsFound.Length; i++)
        {
            if (anthillsFound[i].GetOwner() == owner)
                anthill = anthillsFound[i];
        }
        //Debug.Log(anthill);
    }


    // Food
    public virtual void DecreaseMaxFoodAmount() => maxFoodAmount -= foodIncreaseValue;
    public virtual void IncreseMaxFoodAmount() => maxFoodAmount += foodIncreaseValue;

    public virtual bool AddFood(int value)
    {
        if (foodGathered + value <= maxFoodAmount)
        {
            foodGathered += value;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual bool SpendFood(int value)
    {
        if (foodGathered >= value)
        {
            foodGathered -= value;            
            return true;
        }
        else
            return false;
    }

    public int GetFoodGathered() => foodGathered;
    public int GetMaxFoodAmount() => maxFoodAmount;


    // Game over
    public void QuinDied()
    {
        workersQueued = 0;
        warriorsQueued = 0;
        if (antWorkers.Count > 0)
            foreach (var ant in antWorkers)
            {
                ant.Dead();
            }
        if (antWarriors.Count > 0)
            foreach (var ant in antWarriors)
            {
                ant.Dead();
            }
        Destroy(anthill.gameObject);
    }
}