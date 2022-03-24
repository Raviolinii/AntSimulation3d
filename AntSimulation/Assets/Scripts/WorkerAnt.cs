using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnt : Ant
{
    public GameObject foodPheromone;
    bool lookingForFood = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //MoveInDirection(new Vector3(1,0,0));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pheromone"))
        {
            Debug.Log(other.name);
            var script = other.GetComponent<Pheromone>();
            Debug.Log(script.GetIndex());
        }
    }
    private void OnTriggerExit(Collider other)
    {

    }
}
