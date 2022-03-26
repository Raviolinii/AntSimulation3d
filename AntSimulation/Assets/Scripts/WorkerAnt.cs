using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnt : Ant
{
    public GameObject foodPheromone;
    bool lookingForFood = true;

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
        if (other.CompareTag("Pheromone"))
        {
            var script = other.GetComponent<Pheromone>();
            Debug.Log(script.GetIndex());

            if (lookingForFood)
                script.AddWorkerPheromone(pheromoneLeaveAmount);
            else
                script.AddWorkerFoodPheromone(pheromoneLeaveAmount);
                
            surroundings = script.GetSurroundingsNulls(chosenMoveIndex);
            ChoseMoveIndex();

            //currentTile = script.GetIndex();
        }
    }

    void Move()
    {
        agent.destination = targetTile;
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
                    sum += (int)pheromoneValues[i];
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
                    sum += (int)pheromoneValues[i];
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
        throw new System.Exception("Couldnt find index of element from feromons array");
    }
}
