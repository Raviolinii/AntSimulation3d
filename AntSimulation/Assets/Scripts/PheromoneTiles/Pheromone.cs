using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone
{
    int pheromoneValue = 0;
    public int GetPheromoneValue() => pheromoneValue;
    public void AddPheromone(int value) => pheromoneValue += value;

    public void DecreasePheromone(int value)
    {
        if (pheromoneValue < value)
            pheromoneValue = 0;
        else
            pheromoneValue -= value;
    }

}
