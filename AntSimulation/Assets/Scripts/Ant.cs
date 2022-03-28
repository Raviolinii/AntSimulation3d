using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Ant : MonoBehaviour
{
    protected float speed = 5f;
    protected int chosenMoveIndex;
    protected Vector3 targetTile;
    protected NavMeshAgent agent;
    protected Pheromone[] surroundings = new Pheromone[8];
    protected int pheromoneLeaveAmount = 20;
    protected Pheromone tileScript;
    public Vector3 previousTile;
    protected Vector3 currentTile;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        chosenMoveIndex = Random.Range(0,8);

    }

    // Update is called once per frame
    void Update()
    {

    }
    protected void Move() => agent.destination = targetTile;
    protected void GoToPreviousTile()
    {
        targetTile = previousTile;
        chosenMoveIndex = Mathf.Abs(chosenMoveIndex - 7);
        Move();
    }
}
