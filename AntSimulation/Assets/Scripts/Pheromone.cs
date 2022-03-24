using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone : MonoBehaviour
{
    Vector2 index;
    int workerPheromoneValue = 0;
    int workerFoodPheromoneValue = 0;
    int warriorPheromoneValue = 0;

    Pheromone[] surroundings = new Pheromone[8];
    private void Start()
    {
        
    }

    public Pheromone[] GetSurroundings() => surroundings;
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
    public Vector2 GetIndex() => index;
    public void SetIndex(int i, int j) => index = new Vector2(i, j);
}
