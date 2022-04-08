using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anthill : WorldObject
{
    public Owner _owner;
    public int foodGathered;
    int maxFoodAmount = 300;
    int maxReachableFoodAmount = 2_000;
    int minimalReachableFoodAmount = 300;
    public GameObject queen;

    // Start is called before the first frame update
    void Start()
    {
        stoppingDistance = GetComponentInChildren<SphereCollider>();
        typeOfObject = SpawnedObject.anthill;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("AntWorker"))
        {
            WorkerAnt workerScript = other.GetComponentInParent<WorkerAnt>();
            if (workerScript.WantToStoreFood())
            {
                workerScript.StopAntNearDestination();
                workerScript.GoToPreviousTile();
                workerScript.StoreFood();
            }
        }
    }

    public void SetMaxFoodAmount(int value) => maxFoodAmount = value;
    public int GetMaxFoodAmount() => maxFoodAmount;
    public void DecreaseMaxFoodAmount(int value)
    {
        if (maxFoodAmount - value >= minimalReachableFoodAmount)
            maxFoodAmount -= value;
    }
    public void IncreseMaxFoodAmount(int value)
    {
        if (maxFoodAmount + value <= maxReachableFoodAmount)
            maxFoodAmount += value;
        else
            Debug.Log("Cant");
    }
    public bool AddFood(int value)
    {
        if (foodGathered + value <= maxFoodAmount)
        {
            foodGathered += value;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SpendFood(int value)
    {
        if (foodGathered >= value)
        {
            foodGathered -= value;
            return true;

        }
        else
            return false;
    }
    public int GetFoodGathered() => foodGathered;
    public void SetOwner(Owner owner) => _owner = owner;
    public Owner GetOwner() => _owner;
}