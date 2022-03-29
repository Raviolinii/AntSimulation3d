using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone
{
    //Vector2Int index;
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
    //public Vector2Int GetIndex() => index;
    //public void SetIndex(int i, int j) => index = new Vector2Int(i, j);
}
