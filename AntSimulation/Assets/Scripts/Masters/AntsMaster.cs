using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntsMaster : MonoBehaviour
{

    protected Anthill anthill;
    protected Owner owner;

    // Food
    int maxFoodAmount = 300;
    public int foodGathered;
    int maxReachableFoodAmount = 2_000;
    protected int foodIncreaseValue;

    // Population
    protected int population = 0;
    protected int maxPopulation = 10;
    protected int maxReachablePopulation = 20;
    protected int populationIncreaseValue = 5;

    // Ants
    public GameObject antWorkerPrefab;
    protected List<WorkerAnt> antWorkers = new List<WorkerAnt>();



    // Start is called before the first frame update
    void Start()
    {
        owner = Owner.player;
        Invoke("FindAnthill", 2f);
        Invoke("SetAnthillMaster", 2.2f);
        //Invoke("SpawnWorker", 2.5f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Ants
    protected void SpawnWorker()
    {
        if (CanAddAnt())
        {
            Tile anthillTile = anthill.GetTile();
            Tile[] surroundings = anthillTile.GetSurroundings();
            int maxFoodPheromone = -1;
            int index = 6;
            for (int i = 0; i < 7; i++)
            {
                if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.no)
                {
                    if (maxFoodPheromone < surroundings[i].GetWorkerFoodPheromoneValue())
                    {
                        maxFoodPheromone = surroundings[i].GetWorkerFoodPheromoneValue();
                        index = i;
                    }
                }
            }
            Vector3 position = surroundings[index].transform.position;
            position.y = Terrain.activeTerrain.SampleHeight(position);
            GameObject newAnt = Instantiate(antWorkerPrefab, position, antWorkerPrefab.transform.rotation);
            antWorkers.Add(newAnt.GetComponent<WorkerAnt>());
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