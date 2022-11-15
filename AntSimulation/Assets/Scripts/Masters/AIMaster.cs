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
        var workersCount = antWorkers.Count + workersQueued;
        var warriorsCount = antWarriors.Count + warriorsQueued;

        if (population + warriorsQueued + workersQueued < maxPopulation)
        {
            if (workersCount >= 3 * warriorsCount && foodGathered > warriorPrice)
                BuyWarrior();
            else if (workersCount < 3 * warriorsCount)
                BuyWorker();
        }
        else if(foodGathered > supplyAntPrice)
            BuySupplyAnt();
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
        BuyWorker();
        BuyWorker();
        BuyWorker();
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
