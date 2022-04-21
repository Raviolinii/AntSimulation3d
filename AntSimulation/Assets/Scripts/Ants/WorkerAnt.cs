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

    // Alarm
    public bool dangerSpotted = false;
    Vector3 anthillsPosition;
    Coroutine raiseAlarmCoroutine;


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
        if (other.CompareTag("Tile") && !anthillInRange)
        {
            if (foodInRange && foodScript == null)
                foodInRange = false;

            if (!foodInRange)
            {
                previousTile = currentTile;
                currentTile = other.transform.position;

                tileScript = other.GetComponent<Tile>();
                if (lookingForFood)
                    tileScript.AddWorkerPheromone(pheromoneLeaveAmount);
                else if (!dangerSpotted)
                    tileScript.AddWorkerFoodPheromone(pheromoneLeaveAmount);
                else
                    tileScript.AddWarriorPheromone(pheromoneLeaveAmount);

                surroundings = tileScript.GetSurroundingsNulls(chosenMoveIndex);
                if (!foodInRange && !dangerSpotted)
                {
                    GoToNextTile();
                }
                else if (!foodInRange && dangerSpotted)
                {
                    surroundings = tileScript.GetSurroundings();
                    float lowestDistance = float.MaxValue;
                    Vector3 positionFixed;
                    int index = -1;
                    float distance;
                    for (int i = 0; i < 8; i++)
                    {
                        if (surroundings[i] != null && surroundings[i].GetSpawnedObjectType() == SpawnedObject.no)
                        {
                            positionFixed = surroundings[i].transform.position;
                            positionFixed.y = 0;
                            distance = Vector3.Distance(anthillsPosition, positionFixed);
                            if (distance < lowestDistance)
                            {
                                lowestDistance = distance;
                                index = i;
                            }
                        }
                    }
                    chosenMoveIndex = index;
                    UpdateTargetTile();
                    Move();
                }
            }
        }

        else if (other.CompareTag("AntWorker"))
        {
            WorkerAnt otherScript = other.GetComponent<WorkerAnt>();
            if (otherScript.GetOwner() != _owner && !dangerSpotted)
            {
                Alarm();
            }
        }
    }

    // Alarm
    public void SetAnthillsPosition(Vector3 position)
    {
        anthillsPosition = position;
        anthillsPosition.y = 0;
    }

    void Alarm()
    {
        dangerSpotted = true;
        GoToPreviousTile();

        if (gatherFoodCoroutine != null)
        {
            lookingForFood = false;
            foodInRange = false;
            StopCoroutine(gatherFoodCoroutine);
            gatherFoodCoroutine = null;
            agent.isStopped = false;
        }

    }

    public bool WantToAlarm() => dangerSpotted ? true : false;

    public void RaiseAlarm()
    {
        raiseAlarmCoroutine = StartCoroutine("RaiseAlarmCoroutine");
    }
    IEnumerator RaiseAlarmCoroutine()
    {
        if (gatherFoodCoroutine != null)
        {
            StopCoroutine(gatherFoodCoroutine);
            gatherFoodCoroutine = null;
        }

        yield return new WaitForSeconds(1f);                                            // Tests
        if (anthillScript != null)
            anthillScript.Alarm();

        if (!WantToStoreFood())
        {
            anthillInRange = false;
            lookingForFood = true;
            anthillScript = null;
            agent.isStopped = false;
        }
        dangerSpotted = false;
        raiseAlarmCoroutine = null;
    }

    // Anthill
    public void StoreFood()
    {
        storeFoodCoroutine = StartCoroutine("StoreFoodIEnumerator");
    }

    IEnumerator StoreFoodIEnumerator()
    {
        yield return new WaitForSeconds(storingTime);
        if (anthillInRange == true && anthillScript != null)
        {
            if (anthillScript.AddFood(foodGathered))
                foodGathered = 0;              // should excess food be destroyed?? 

            anthillInRange = false;
            lookingForFood = true;
            anthillScript = null;
            agent.isStopped = false;
        }
        storeFoodCoroutine = null;
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

    public bool WantToStoreFood() => anthillInRange ? true : false;
    //public bool WantToStoreFood() => anthillInRange && foodGathered > 0 ? true : false;


    // Food
    public void GatherFood()
    {
        gatherFoodCoroutine = StartCoroutine("GatherFoodIEnumerator");
    }

    IEnumerator GatherFoodIEnumerator()
    {
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
        gatherFoodCoroutine = null;
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

    public bool WantToGather() => foodInRange ? true : false;




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
        if (foodInRange || anthillInRange || dangerSpotted)
            agent.isStopped = true;
    }

}
