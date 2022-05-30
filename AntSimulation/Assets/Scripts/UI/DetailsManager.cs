using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsManager : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    public GameObject anthillDetailsPrefab;
    public GameObject details;



    // Anthill Details
    public void UpdateAnthillDetailsQueuesTexts(int workers, int warriors, bool supply)
    {
        if (details != null && details.CompareTag("MyAnthillDetails"))
        {
            var contentPanel = details.transform.GetChild(1);
            var queuePanel = contentPanel.transform.GetChild(1);
            var textMeshes = queuePanel.GetComponentsInChildren<TextMeshProUGUI>();

            textMeshes[0].text = workers.ToString();
            textMeshes[1].text = warriors.ToString();
            textMeshes[2].text = supply.ToString();
        }
    }

    public void ShowAnthillDetails(AntsMaster master)
    {
        if (details != null)
        {
            if (details.CompareTag("MyAnthillDetails"))
                return;

            CloseDetails();
        }

        details = Instantiate(anthillDetailsPrefab, canvas.transform);
        var buttons = details.GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(() => { master.BuyWorker(); });
        buttons[1].onClick.AddListener(() => { master.BuyWarrior(); });
        buttons[2].onClick.AddListener(() => { master.BuySupplyAnt(); });

        var contentPanel = details.transform.GetChild(1);
        var queuePanel = contentPanel.transform.GetChild(1);

        var textMeshes = queuePanel.GetComponentsInChildren<TextMeshProUGUI>();

        var workers = master.GetWorkersQueued();
        var warriors = master.GetWarriorsQueued();
        var supply = master.ISSupplyAntQueued();

        UpdateAnthillDetailsQueuesTexts(workers, warriors, supply);
    }

    public void CloseDetails()
    {
        if (details != null)
            Destroy(details);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
