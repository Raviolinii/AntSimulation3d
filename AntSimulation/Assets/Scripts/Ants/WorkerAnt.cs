using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnt : Ant
{

    // Food
    public bool lookingForFood = true;
    public bool foodInRange = false;
    Food foodScript;
    int gatheringAmount = 20;
    float gatheringTime = 2.5f;
    int foodGathered = 0;
    Coroutine gatherFoodCoroutine;


    // Anthill
    public bool anthillInRange = false;
    Anthill anthillScript;
    float storingTime = 1.5f;
    Coroutine storeFoodCoroutine;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hp = 10;
        dmg = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveInDirection(new Vector3(1,0,0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile") && !foodInRange && !anthillInRange)
        {
            previousTile = currentTile;
            currentTile = other.transform.position;

            tileScript = other.GetComponent<Tile>();
            if (lookingForFood)
                tileScript.AddWorkerPheromone(pheromoneLeaveAmount);
            else
                tileScript.AddWorkerFoodPheromone(pheromoneLeaveAmount);

            surroundings = tileScript.GetSurroundingsNulls(chosenMoveIndex);
            if (!foodInRange)
            {
                GoToNextTile();
            }
        }
    }


    // Anthill
    public void StoreFood()
    {
        storeFoodCoroutine = StartCoroutine("StoreFoodIEnumerator");
    }

    IEnumerator StoreFoodIEnumerator()
    {
        Debug.Log("Storing");
        yield return new WaitForSeconds(storingTime);
        if (anthillScript != null)
        {
            if (anthillScript.AddFood(foodGathered))
                foodGathered = 0;              // should excess food be destroyed?? 

            anthillInRange = false;
            Debug.Log("Got it");
            lookingForFood = true;
            anthillScript = null;              // null food script
            agent.isStopped = false;
        }
    }

    void AnthillFound(Anthill anthill)
    {
        if (!anthillInRange)
        {
            anthillScript = anthill;
            anthillInRange = true;
            UpdateTargetTile();
            Move();
        }
    }

    public bool WantToGather() => foodInRange ? true : false;


    // Food
    public void GatherFood()
    {
        gatherFoodCoroutine = StartCoroutine("GatherFoodIEnumerator");
    }

    IEnumerator GatherFoodIEnumerator()
    {
        Debug.Log("Gathering");
        yield return new WaitForSeconds(gatheringTime);
        if (foodScript != null)
        {
            foodGathered = foodScript.GatherFood(gatheringAmount);
            foodScript = null;              // null food script
        }
        else
            GoToPreviousTile();

        foodInRange = false;
        lookingForFood = false;
        agent.isStopped = false;
    }

    void FoundFood(Food food)
    {
        if (!foodInRange)
        {
            foodInRange = true;
            foodScript = food;
            UpdateTargetTile();
            Move();
        }
    }

    public bool WantToStoreFood() => anthillInRange ? true : false;


    // Movement
    public void SetTargetTile(Vector3 target) => targetTile = target;

    protected override void ChoseMoveIndex()
    {
        // RouletteTileSelection

        int?[] pheromoneValues = new int?[8];
        int sum = 0;
        if (lookingForFood)
            LookingForFoodMoveIndex(pheromoneValues, sum);

        else if (!lookingForFood)
            NotLookingForFoodMoveIndex(pheromoneValues, sum);

    }

    void NotLookingForFoodMoveIndex(int?[] pheromoneValues, int sum)
    {
        int rand;
        for (int i = 0; i < 8; i++)
        {
            if (surroundings[i] != null && ((int)surroundings[i].GetSpawnedObjectType() < 2))   // 0 is a free space, 1 is anthill, 2 is food, 3 is obstacle
            {
                if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.anthill)
                {
                    Anthill anthill = (Anthill)surroundings[i].GetSpawnedObject();
                    if (anthill.GetOwner() == _owner)
                    {
                        AnthillFound(anthill);
                        chosenMoveIndex = i;
                        return;
                    }
                }
                pheromoneValues[i] = surroundings[i].GetWorkerPheromoneValue();
                sum += (int)pheromoneValues[i] + 1;
                pheromoneValues[i] = sum;
            }
        }

        rand = Random.Range(1, sum + 1);
        int index = FindIndex(pheromoneValues, rand);
        ChosenIndexValidation(index);
    }

    void LookingForFoodMoveIndex(int?[] pheromoneValues, int sum)
    {
        int rand;
        for (int i = 0; i < 8; i++)
        {
            if (surroundings[i] != null)
            {
                if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.food)
                {
                    Food food = (Food)surroundings[i].GetSpawnedObject();
                    FoundFood(food);
                    chosenMoveIndex = i;
                    return;
                }
                else if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.no)
                {
                    pheromoneValues[i] = surroundings[i].GetWorkerFoodPheromoneValue();
                    sum += (int)pheromoneValues[i] + 1;
                    pheromoneValues[i] = sum;
                }
            }
        }

        rand = Random.Range(1, sum + 1);
        int index = FindIndex(pheromoneValues, rand);
        ChosenIndexValidation(index);
    }

    public void StopAntNearDestination()
    {
        if (foodInRange || anthillInRange)
            agent.isStopped = true;
    }

}
