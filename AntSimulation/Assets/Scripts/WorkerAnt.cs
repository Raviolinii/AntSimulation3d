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

        if (other.CompareTag("Food") && lookingForFood && !foodInRange)
        {
            foodInRange = true;
            targetTile = other.transform.position;
            Move();
            foodScript = other.GetComponent<Food>();
        }
        if (other.CompareTag("Anthill") && !lookingForFood && !anthillInRange)
        {
            Debug.Log("Anthill");
            anthillInRange = true;
            targetTile = other.transform.position;
            Move();
            anthillScript = other.GetComponent<Anthill>();
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
                foodGathered = 0;

            anthillInRange = false;
            Debug.Log("Got it");
            lookingForFood = true;
            anthillScript = null;              // null food script
            GoToPreviousTile();
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
            foodInRange = false;
            Debug.Log("Got it");
            lookingForFood = false;
            foodScript = null;              // null food script
            GoToPreviousTile();
            agent.isStopped = false;
        }
    }

    void UpdateTargetTile() => targetTile = surroundings[chosenMoveIndex].transform.position;
    void ChoseMoveIndex()
    {
        // RouletteTileSelection

        int?[] pheromoneValues = new int?[8];
        if (lookingForFood)
        {
            int sum = 0;
            int rand;
            for (int i = 0; i < 8; i++)
            {
                if (surroundings[i] != null)
                {
                    pheromoneValues[i] = surroundings[i].GetWorkerFoodPheromoneValue();
                    sum += (int)pheromoneValues[i] + 1;
                    pheromoneValues[i] = sum;
                }
            }

            rand = Random.Range(1, sum + 1);
            chosenMoveIndex = FindIndex(pheromoneValues, rand);
        }
        else if (!lookingForFood)
        {
            int sum = 0;
            int rand;
            for (int i = 0; i < 8; i++)
            {
                if (surroundings[i] != null)
                {
                    pheromoneValues[i] = surroundings[i].GetWorkerPheromoneValue();
                    sum += (int)pheromoneValues[i] + 1;
                    pheromoneValues[i] = sum;
                }
            }

            rand = Random.Range(1, sum + 1);
            chosenMoveIndex = FindIndex(pheromoneValues, rand);
        }
    }

    protected int FindIndex(int?[] array, int value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null)
            {
                if (value <= array[i])
                    return i;
            }
        }
        // need to go back
        int result = Mathf.Abs(chosenMoveIndex - 7);
        AddPreviousTile(result);
        return result;

        //throw new System.Exception("Couldnt find index of element from feromons array");
    }

    void AddPreviousTile(int index) => surroundings[index] = tileScript.GetTile(index);
    public void StopAntNearDestination()
    {
        if (foodInRange || anthillInRange)
            agent.isStopped = true;
    }
    public bool WantToGather() => foodInRange ? true : false;
    public bool WantToStoreFood() => anthillInRange ? true : false;
}
