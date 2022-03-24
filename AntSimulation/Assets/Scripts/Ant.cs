using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    float speed = 5f;
    public GameObject pheromone;
    protected List<Pheromone> pheromonesDetected;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void MoveInDirection(Vector3 direction)
    {
        transform.Translate(direction * (speed * Time.deltaTime));
    }
}
