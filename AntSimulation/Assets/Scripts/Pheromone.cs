using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone : MonoBehaviour
{
    Vector2Int index;
    int workerPheromoneValue = 0;
    int workerFoodPheromoneValue = 0;
    int warriorPheromoneValue = 0;

    public Pheromone[] surroundings = new Pheromone[8];
    private void Start()
    {
        
    }

    public Pheromone[] GetSurroundingsNulls(int chosenMoveIndex)
    {
        Pheromone[] result = (Pheromone[]) surroundings.Clone();
        switch (chosenMoveIndex)
        {
            case 0:
            {
                result[4] = null;
                result[6] = null;
                result[7] = null;
                break;
            }
            case 1:
            {
                result[5] = null;
                result[6] = null;
                result[7] = null;
                break;
            }
            case 2:
            {
                result[3] = null;
                result[5] = null;
                result[6] = null;
                break;
            }
            case 3:
            {
                result[2] = null;
                result[4] = null;
                result[7] = null;
                break;
            }
            case 4:
            {
                result[2] = null;
                result[3] = null;
                result[5] = null;
                break;
            }
            case 5:
            {
                result[1] = null;
                result[2] = null;
                result[4] = null;
                break;
            }
            case 6:
            {
                result[0] = null;
                result[1] = null;
                result[2] = null;
                break;
            }
            case 7:
            {
                result[0] = null;
                result[1] = null;
                result[5] = null;
                break;
            }

            default:
                throw new System.Exception("Invalid chosenMoveIndex in GetSurroundings method");
        }
        return result;
    }
    public Pheromone[] GetSurroundings() => surroundings;
    public Pheromone GetTile(int index) => surroundings[index];
    public int GetWorkerPheromoneValue() => workerPheromoneValue;
    public int GetWorkerFoodPheromoneValue() => workerFoodPheromoneValue;
    public int GetWarriorPheromoneValue() => warriorPheromoneValue;

    public void AddToSurroundings(Pheromone toAdd, int position) => surroundings[position] = toAdd;
    public void AddWorkerPheromone(int value) => workerPheromoneValue += value;
    public void AddWorkerFoodPheromone(int value) => workerFoodPheromoneValue += value;
    public void AddWarriorPheromone(int value) => warriorPheromoneValue += value;

    public void DecreaseWorkerPheromone(int value)
    {
        if (workerPheromoneValue < value)
            workerPheromoneValue = 0;
        else
            workerPheromoneValue -= value;
    }
    public void DecreaseWorkerFoodPheromoneValue(int value)
    {
        if (workerFoodPheromoneValue < value)
            workerFoodPheromoneValue = 0;
        else
            workerFoodPheromoneValue -= value;
    }
    public void DecreaseWarriorPheromoneValue(int value)
    {
        if (warriorPheromoneValue < value)
            warriorPheromoneValue = 0;
        else
            warriorPheromoneValue -= value;
    }
    public void DecreasePheromones(int value)
    {
        DecreaseWorkerPheromone(value);
        DecreaseWorkerFoodPheromoneValue(value);
        DecreaseWarriorPheromoneValue(value);
    }
    public Vector2Int GetIndex() => index;
    public void SetIndex(int i, int j) => index = new Vector2Int(i, j);
}
