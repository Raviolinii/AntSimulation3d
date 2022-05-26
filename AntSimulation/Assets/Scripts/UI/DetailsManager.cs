using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsManager : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    public GameObject anthillDetailsPrefab;
    public GameObject details;


    public void ShowAnthillDetails()
    {
        if (details == null)
            details = Instantiate(anthillDetailsPrefab, canvas.transform);
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
