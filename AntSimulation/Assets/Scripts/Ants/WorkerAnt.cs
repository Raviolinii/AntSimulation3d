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
    float gatheringTime = 4.2f;
    public int foodGathered = 0;
    Coroutine gatherFoodCoroutine;


    // Anthill
    public bool anthillInRange = false;
    public Anthill anthillScript;
    float storingTime = 5f;
    Coroutine storeFoodCoroutine;

    // Alarm
    public bool dangerSpotted = false;
    Vector3 anthillsPosition;
    Coroutine raiseAlarmCoroutine;

    // Movement
    float movementRange = 50;

    // Animations
    protected const string isStoring = "IsStoring";
    protected const string isGathering = "IsGathering";


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
        if (other.CompareTag("Tile"))
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
            if (foodInRange && foodScript == null)
                foodInRange = false;

            if (!anthillInRange)
            {
                if (!foodInRange)
                {
                    /*                 previousTile = currentTile;
                                    currentTile = other.transform.position;

                                    tileScript = other.GetComponent<Tile>();
                                    if (lookingForFood)
                                        tileScript.AddWorkerPheromone(pheromoneLeaveAmount);
                                    else if (!dangerSpotted)
                                        tileScript.AddWorkerFoodPheromone(pheromoneLeaveAmount);
                                    else
                                        tileScript.AddWarriorPheromone(pheromoneLeaveAmount); */

                    surroundings = tileScript.GetSurroundingsNulls(chosenMoveIndex);
                    if (!dangerSpotted)
                    {
                        GoToNextTile();
                    }
                    else
                    {
                        MoveCloserToAnthill();
                    }
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

        else if (other.CompareTag("Anthill"))
        {
            anthillScript = other.GetComponent<Anthill>();

            if (anthillScript.GetOwner() == _owner)
            {

                if (WantToAlarm())
                {
                    StopAntNearDestination();
                    GoToPreviousTile();
                    RaiseAlarm();
                }

                if (WantToStoreFood())
                {
                    StopAntNearDestination();
                    GoToPreviousTile();
                    StoreFood();
                }
            }
        }
        else if (other.CompareTag("Food"))
        {
            if (WantToGather())
            {
                foodScript = other.GetComponent<Food>();
                StopAntNearDestination();
                GoToPreviousTile();
                GatherFood();
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
            animator.SetBool(isGathering, false);
            gatherFoodCoroutine = null;
            agent.isStopped = false;
            animator.SetBool(isMoving, true);
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
            animator.SetBool(isGathering, false);
            gatherFoodCoroutine = null;
        }

        animator.SetBool(isGathering, true);
        yield return new WaitForSeconds(storingTime);                                            // Tests
        animator.SetBool(isGathering, false);
        if (anthillScript != null)
            anthillScript.Alarm();

        if (!WantToStoreFood())
        {
            anthillInRange = false;
            lookingForFood = true;
            anthillScript = null;
            agent.isStopped = false;
            animator.SetBool(isMoving, true);
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
        animator.SetBool(isStoring, true);
        yield return new WaitForSeconds(storingTime);
        animator.SetBool(isStoring, false);

        if (anthillInRange == true && anthillScript != null)
        {
            if (anthillScript.AddFood(foodGathered))
                foodGathered = 0;              // should excess food be destroyed?? 

            anthillInRange = false;
            lookingForFood = true;
            anthillScript = null;
            agent.isStopped = false;
            animator.SetBool(isMoving, true);
        }
        storeFoodCoroutine = null;
    }

    void AnthillFound()
    {
        if (!anthillInRange)
        {
            anthillInRange = true;
            UpdateTargetTile();
            Move();
        }
    }

    //public bool WantToStoreFood() => anthillInRange ? true : false;
    public bool WantToStoreFood() => anthillInRange && foodGathered > 0 ? true : false;


    // Food
    public void GatherFood()
    {
        gatherFoodCoroutine = StartCoroutine("GatherFoodIEnumerator");
    }

    IEnumerator GatherFoodIEnumerator()
    {
        animator.SetBool(isGathering, true);
        yield return new WaitForSeconds(gatheringTime);
        animator.SetBool(isGathering, false);

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
        animator.SetBool(isMoving, true);
        gatherFoodCoroutine = null;
    }

    void FoundFood()
    {
        if (!foodInRange)
        {
            foodInRange = true;
            UpdateTargetTile();
            Move();
        }
    }

    public bool WantToGather() => foodInRange ? true : false;


    // Movement
    void MoveCloserToAnthill()
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

    public void SetTargetTile(Vector3 target) => targetTile = target;
    protected override void ChosenIndexValidation(int index)
    {
        if (index != -1)
            chosenMoveIndex = index;
        else
        {
            MoveCloserToAnthill();
        }
    }

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
        float distance;
        Vector3 tilePosition;
        for (int i = 0; i < 8; i++)
        {
            if (surroundings[i] != null && ((int)surroundings[i].GetSpawnedObjectType() < 2))   // 0 is a free space, 1 is anthill, 2 is food, 3 is obstacle
            {
                if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.anthill)
                {
                    Anthill anthill = (Anthill)surroundings[i].GetSpawnedObject();
                    if (anthill.GetOwner() == _owner)
                    {
                        AnthillFound();
                        chosenMoveIndex = i;
                        return;
                    }
                }
                tilePosition = surroundings[i].transform.position;
                tilePosition.y = 0;
                distance = Vector3.Distance(anthillsPosition, tilePosition);

                if (distance <= movementRange)
                {
                    pheromoneValues[i] = surroundings[i].GetWorkerPheromoneValue();
                    sum += (int)pheromoneValues[i] + 1;
                    pheromoneValues[i] = sum;
                }
                else
                    pheromoneValues[i] = null;
            }
        }

        rand = Random.Range(1, sum + 1);
        int index = FindIndex(pheromoneValues, rand);
        ChosenIndexValidation(index);
    }

    void LookingForFoodMoveIndex(int?[] pheromoneValues, int sum)
    {
        int rand;
        float distance;
        Vector3 tilePosition;
        for (int i = 0; i < 8; i++)
        {
            if (surroundings[i] != null)
            {
                if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.food)
                {
                    Food food = (Food)surroundings[i].GetSpawnedObject();
                    FoundFood();
                    chosenMoveIndex = i;
                    return;
                }
                else if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.no)
                {
                    tilePosition = surroundings[i].transform.position;
                    tilePosition.y = 0;
                    distance = Vector3.Distance(anthillsPosition, tilePosition);

                    if (distance <= movementRange)
                    {
                        pheromoneValues[i] = surroundings[i].GetWorkerFoodPheromoneValue();
                        sum += (int)pheromoneValues[i] + 1;
                        pheromoneValues[i] = sum;
                    }
                    else
                        pheromoneValues[i] = null;
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
        {
            agent.isStopped = true;
            animator.SetBool(isMoving, false);
        }
    }

    public void SetMovementRange(int value) => movementRange = value;
}
