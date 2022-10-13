using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : WorldObject
{
    object _ = new object();
    protected int amount;
    PheromoneMap map;

    // Start is called before the first frame update
    void Start()
    {
        stoppingDistance = GetComponentInChildren<SphereCollider>();
        typeOfObject = SpawnedObject.food;
        amount = Random.Range(200, 601);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetFoodAmount() => amount;
    public int GatherFood(int value)
    {
        int decresedBy = 0;
        lock (_)
        {
            if (amount < value)
            {
                decresedBy = amount;
                amount = 0;
                Depleted();
            }
            else
            {
                decresedBy = value;
                amount -= value;
            }
        }
        return decresedBy;
    }
    void Depleted()
    {
        _tile.ObjectDestroyed();
        map.DecreaseFoodCount();
        Destroy(gameObject);
    }

    public void SetPheromoneMap(PheromoneMap pheromoneMap) => map = pheromoneMap;
}
