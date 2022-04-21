using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntsMaster : MonoBehaviour
{

    protected Anthill anthill;
    protected Owner owner;


    // Food
    int maxFoodAmount = 540;
    public int foodGathered;
    int maxReachableFoodAmount = 2_000;
    protected int foodIncreaseValue;


    // Population
    protected int population = 0;
    protected int maxPopulation = 10;
    protected int maxReachablePopulation = 50;
    protected int populationIncreaseValue = 5;


    // Ants
    public GameObject antWorkerPrefab;
    public GameObject antWarriorPrefab;
    protected List<WorkerAnt> antWorkers = new List<WorkerAnt>();
    public int warriorsStacked = 0;
    protected List<AntWarrior> antWarriors = new List<AntWarrior>();


    // Buy Ants
    int workerPrice = 60;
    int warriorPrice = 120;


    // Alarm
    public bool dangerSpotted = false;


    // Start is called before the first frame update
    void Start()
    {
        owner = Owner.player;
        Invoke("FindAnthill", 2f);
        Invoke("SetAnthillMaster", 2.2f);
        //Invoke("SpawnWorker", 2.5f);
        //Invoke("SpawnWarrior", 2.5f);
        AddFood(540);
        Invoke("BuyWorker", 2.5f);
        Invoke("BuyWarrior", 2.5f);        
        Invoke("BuyWorker", 4.5f);
        Invoke("BuyWarrior", 4.5f);        
        Invoke("BuyWorker", 6.5f);
        Invoke("BuyWarrior", 6.5f);
        //Invoke("Zerg", 2.5f);

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
    public void BuyWorker()
    {
        if (CanAddAnt() && foodGathered >= workerPrice)
        {
            foodGathered -= workerPrice;
            SpawnWorker();
        }
    }

    public void BuyWarrior()
    {
        if (CanAddAnt() && foodGathered >= warriorPrice)
        {
            foodGathered -= warriorPrice;

            if (!dangerSpotted)
                warriorsStacked++;
            
            else
                SpawnWarrior();
        }
    }

    // Alarm
    public void Alarm()
    {
        dangerSpotted = true;
        if (warriorsStacked > 0)
        {
            for (int i = 0; i < warriorsStacked; i++)
            {
                SpawnWarrior();
            }
        }
    }

    // Ants
    Vector3 AsignYPosition(Vector3 position)
    {
        position.y = Terrain.activeTerrain.SampleHeight(position);
        return position;
    }

    protected void SpawnWorker()
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
            antWorkers.Add(newAntScript);
            IncreasePopulation();
        }
    }

    protected void SpawnWarrior()
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
            antWarriors.Add(newAntScript);
            IncreasePopulation();
        }
    }


    // Population
    public int GetPopulation() => population;
    public void IncreasePopulation(int value) => population += value;
    public void IncreasePopulation() => population++;
    public void DecreasePopulation(int value) => population -= value;
    public void DecreasePopulation() => population--;
    public bool CanAddAnt() => population < maxPopulation ? true : false;
    public void IncreaseMaxPopulation()
    {
        if (maxPopulation + populationIncreaseValue <= maxReachablePopulation)
            maxPopulation += populationIncreaseValue;
    }

    public void DecreaseMaxPopulation() => maxPopulation -= populationIncreaseValue;


    // Initialization
    protected void SetAnthillMaster() => anthill.SetMaster(this);
    protected void FindAnthill()
    {
        var anthillsFound = FindObjectsOfType<Anthill>();
        for (int i = 0; i < anthillsFound.Length; i++)
        {
            if (anthillsFound[i].GetOwner() == owner)
                anthill = anthillsFound[i];
        }
        Debug.Log(anthill);
    }


    // Food
    public void DecreaseMaxFoodAmount() => maxFoodAmount -= foodIncreaseValue;
    public void IncreseMaxFoodAmount()
    {
        if (maxFoodAmount + foodIncreaseValue <= maxReachableFoodAmount)
            maxFoodAmount += foodIncreaseValue;
        else
            Debug.Log("Cant");
    }

    public bool AddFood(int value)
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

    public bool SpendFood(int value)
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
}