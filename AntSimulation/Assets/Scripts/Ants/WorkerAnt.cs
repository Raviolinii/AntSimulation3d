using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnt : Ant
{
    public bool lookingForFood = true;
    public bool foodInRange = false;
    public bool anthillInRange = false;
    float gatheringTime = 2.5f;
    float storingTime = 1.5f;
    Food foodScript;
    Anthill anthillScript;
    int gatheringAmount = 20;
    Coroutine gatherFoodCoroutine;
    Coroutine storeFoodCoroutine;
    int foodGathered = 0;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetOwner(Owner.player);
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
                ChoseMoveIndex();
                UpdateTargetTile();
                Move();
            }

            //currentTile = script.GetIndex();
        }
    }

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
    void LookingForFoodMoveIndex(int?[] pheromoneValues, int sum)                   // doesnt care about obstacles and want go through anthill
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




    //void AddPreviousTile(int index) => surroundings[index] = tileScript.GetTile(index);
    public void StopAntNearDestination()
    {
        if (foodInRange || anthillInRange)
            agent.isStopped = true;
    }
    public bool WantToGather() => foodInRange ? true : false;
    public bool WantToStoreFood() => anthillInRange ? true : false;

}
