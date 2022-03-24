using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    float speed = 5f;
    public GameObject pheromone;
    List<Pheromone> pheromonesDetected;
    int leftPheromones;
    int rightPheromones;
    int forwardPheromones;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(1, 0, 0) * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other) 
    {
        
    }

    public void AddLeftPheromone() => leftPheromones += 1;
    public void AddRightPheromone() => leftPheromones += 1;
    public void AddForwardPheromone() => leftPheromones += 1;
    public void DecreseLeftPheromone() => leftPheromones -= 1;
    public void DecreseRightPheromone() => leftPheromones -= 1;
    public void DecreseForwardPheromone() => leftPheromones -= 1;
}
