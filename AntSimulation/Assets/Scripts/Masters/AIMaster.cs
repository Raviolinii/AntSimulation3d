using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMaster : AntsMaster
{
    // Start is called before the first frame update
    protected override void Start()
    {
        owner = Owner.AI;
        base.Start();

        //Invoke("BuyFirst8Ants", 2.5f);
        InvokeRepeating("Add200Food", 7f, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(foodGathered > 60)
            TryBuyAnts();
    }

    public void Add200Food() => AddFood(200);

    private void TryBuyAnts()
    {
        var workersCount = antWorkers.Count;
        var warriorsCount = antWarriors.Count + warriorsQueued;

        if (population < maxPopulation)
        {
            if (workersCount >= 3 * warriorsCount && foodGathered > warriorPrice)
                BuyWarrior();
            else if (workersCount < 3 * warriorsCount)
                BuyWorker();
        }
        else if(foodGathered > supplyAntPrice)
            BuySupplyAnt();

        Debug.Log($"workers : {workersCount}, warriors : {warriorsCount}, supply : {supplyAnts}");
        Debug.Log(foodGathered);
    }

    // Food
    public override bool AddFood(int value)
    {
        bool added = base.AddFood(value);
        return added;
    }

    // Buy first ants
    protected override void SetInitialFoodAmount()
    {
        base.SetInitialFoodAmount();
        BuyFirst8Ants();
    }

    private void BuyFirst8Ants()
    {
        Debug.Log(foodGathered);
        BuyWorker();
        Debug.Log(foodGathered);
        BuyWorker();
        Debug.Log(foodGathered);
        BuyWorker();
        Debug.Log(foodGathered);
        BuyWorker();
        BuyWorker();
        BuyWorker();
        BuyWorker();
        BuyWorker();
    }
    private void BuyFirst6Ants()
    {
        BuyWorker();
        BuyWorker();
        BuyWorker();
        BuyWorker();
        BuyWorker();
        BuySupplyAnt();
    }
}
