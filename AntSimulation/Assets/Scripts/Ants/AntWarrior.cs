using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntWarrior : Ant
{
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //SetOwner(Owner.AI);
        hp = 10;
        dmg = 5;
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            previousTile = currentTile;
            currentTile = other.transform.position;

            tileScript = other.GetComponent<Tile>();
            tileScript.AddWarriorPheromone(pheromoneLeaveAmount);
            surroundings = tileScript.GetSurroundingsNulls(chosenMoveIndex);

            GoToNextTile();
        }

        if (other.CompareTag("AntWorker") || other.CompareTag("AntWarrior"))
        {
            AntDetected(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AntWorker") || other.CompareTag("AntWarrior"))
        {
            AntDetected(other);
        }
    }


    // Move
    protected override void ChoseMoveIndex()
    {
        int?[] pheromoneValues = new int?[8];
        int sum = 0;
        int rand;
        for (int i = 0; i < 8; i++)
        {
            if (surroundings[i] != null)
            {
                if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.anthill)
                {
                    Anthill anthillScript = surroundings[i].GetSpawnedObject().GetComponent<Anthill>();
                    if (anthillScript.GetOwner() != _owner)
                    {
                        // Attack anthill
                    }
                }
                else if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.no)
                {
                    pheromoneValues[i] = surroundings[i].GetWarriorPheromoneValue();
                    sum += (int)pheromoneValues[i] + 1;
                    pheromoneValues[i] = sum;
                }
            }
        }

        rand = Random.Range(1, sum + 1);
        int index = FindIndex(pheromoneValues, rand);
        ChosenIndexValidation(index);
    }


    // Fight
    void AntDetected(Collider ant)
    {
        var antScript = ant.GetComponent<Ant>();
        if (antScript.GetOwner() == _owner)
            return;

        Fight(antScript);
        antScript.Fight(this);
    }
}
