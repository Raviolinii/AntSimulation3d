using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsManager : MonoBehaviour
{

    // Anthill Details
    public GameObject anthillDetailsPrefab;
    AntsMaster _master;

    // Details
    [SerializeField]
    private Canvas canvas;
    public GameObject details;


    // Anthill Details
    public void AnthillSliderValueChanged(float value) => _master.SetMovementRange((int)value);

    public void UpdateAnthillDetailsQueuesTexts(int workers, int warriors, bool supply)
    {
        if (details != null && details.CompareTag("MyAnthillDetails"))
        {
            var contentPanel = details.transform.GetChild(1);
            var queuePanel = contentPanel.transform.GetChild(1);
            var textMeshes = queuePanel.GetComponentsInChildren<TextMeshProUGUI>();

            textMeshes[0].text = $"Workers queued: {workers.ToString()}";
            textMeshes[1].text = $"Warriors queued: {warriors.ToString()}";
            if (supply)
                textMeshes[2].text = $"Supply Ant queued";
            else
                textMeshes[2].text = $"Supply Ant not queued";

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

        if (_master == null)
            _master = master;

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

        var slider = details.GetComponentInChildren<Slider>();

        slider.onValueChanged.AddListener((value) => { AnthillSliderValueChanged(value); });
    }


    // Details
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
