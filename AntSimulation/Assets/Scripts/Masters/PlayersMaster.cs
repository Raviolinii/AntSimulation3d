using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayersMaster : AntsMaster
{
    // Selection
    [SerializeField]
    LayerMask layerMask;

    // UI Details
    [SerializeField]
    DetailsManager detailsManager;


    // UI Texts
    public TextMeshProUGUI populationTMP;
    public TextMeshProUGUI workersTMP;
    public TextMeshProUGUI warriorsTMP;
    public TextMeshProUGUI foodTMP;


    // Start is called before the first frame update
    protected override void Start()
    {
        owner = Owner.player;
        base.Start();
        UpdateFoodCountText(maxFoodAmount);
        UpdatePopulationCountText(population);
        UpdateWarriorsCountText(antWarriors.Count);
        UpdateWorkersCountText(antWorkers.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Select"))
            Selection();
    }


    // Selection
    void Selection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //detailsManager.CloseDetails();
        if (Physics.Raycast(ray, out hit, 300.0f, layerMask))
        {
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.CompareTag("Anthill"))
                {
                    var script = hit.transform.GetComponent<Anthill>();
                    if (script.GetOwner() == this.owner)
                    {
                        Debug.Log("Thats my anthill");
                        detailsManager.ShowAnthillDetails(this);
                    }
                    else
                    {
                        Debug.Log("Enemy...");
                        detailsManager.CloseDetails();
                    }
                }
            }
        }
    }


    // UI Details

    public void UpdateDetailsQueuesTexts()
    {
        detailsManager.UpdateAnthillDetailsQueuesTexts(workersQueued, warriorsQueued, supplyAntQueued);
    }

    // UI Texts
    public void UpdatePopulationCountText(int count) => populationTMP.text = $"Population {count}/{maxPopulation}";
    public void UpdateFoodCountText(int count) => foodTMP.text = $"Food {count}/{maxFoodAmount}";
    public void UpdateWarriorsCountText(int count) => warriorsTMP.text = $"Warriors {count}";
    public void UpdateWorkersCountText(int count) => workersTMP.text = $"Workers {count}";


    // Buy Ants
    public override void BuyWorker()
    {
        base.BuyWorker();
        UpdateDetailsQueuesTexts();
    }

    public override void BuyWarrior()
    {
        base.BuyWarrior();
        UpdateDetailsQueuesTexts();
    }

    public override void BuySupplyAnt()
    {
        base.BuySupplyAnt();
        UpdateDetailsQueuesTexts();
    }


    // Coroutine Ants
    protected override IEnumerator WorkerSpawnIEnumerator()
    {
        yield return base.WorkerSpawnIEnumerator();
            UpdateDetailsQueuesTexts();
    }

    protected override IEnumerator WarriorSpawnIEnumerator()
    {
        yield return base.WarriorSpawnIEnumerator();
            UpdateDetailsQueuesTexts();
    }

    protected override IEnumerator SupplyAntSpawnIEnumerator()
    {
        yield return base.SupplyAntSpawnIEnumerator();
            UpdateDetailsQueuesTexts();
    }

    // Ants
    protected override void SpawnWorker()
    {
        base.SpawnWorker();
        UpdateWorkersCountText(antWorkers.Count);
    }

    protected override void SpawnWarrior()
    {
        base.SpawnWarrior();
        UpdateWarriorsCountText(antWarriors.Count);
    }


    // Population
    public override void IncreasePopulation(int value)
    {
        base.IncreasePopulation(value);
        UpdatePopulationCountText(population);
    }

    public override void IncreasePopulation()
    {
        base.IncreasePopulation();
        UpdatePopulationCountText(population);
    }

    public override void DecreasePopulation(int value)
    {
        base.DecreasePopulation(value);
        UpdatePopulationCountText(population);
    }

    public override void DecreasePopulation()
    {
        base.DecreasePopulation();
        UpdatePopulationCountText(population);
    }

    public override void IncreaseMaxPopulation()
    {
        base.IncreaseMaxPopulation();
        UpdatePopulationCountText(population);
    }

    public override void DecreaseMaxPopulation()
    {
        base.DecreaseMaxPopulation();
        UpdatePopulationCountText(population);
    }


    // Food
    public override void DecreaseMaxFoodAmount()
    {
        base.DecreaseMaxFoodAmount();
        UpdateFoodCountText(foodGathered);
    }

    public override void IncreseMaxFoodAmount()
    {
        base.IncreseMaxFoodAmount();
        UpdateFoodCountText(foodGathered);
    }

    public override bool AddFood(int value)
    {
        var result = base.AddFood(value);
        if (result)
            UpdateFoodCountText(foodGathered);

        return result;
    }

    public override bool SpendFood(int value)
    {
        var result = base.SpendFood(value);
        if (result)
            UpdateFoodCountText(foodGathered);

        return result;
    }


}
