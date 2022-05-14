using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Ant : MonoBehaviour
{
    // Owner
    public Owner _owner;
    protected AntsMaster _master;


    // Movement
    protected float speed = 5f;
    public int chosenMoveIndex;
    protected NavMeshAgent agent;


    // Tiles and positions
    protected Vector3 targetTile;
    public Tile[] surroundings = new Tile[8];
    protected Tile tileScript;
    public Vector3 previousTile;
    protected Vector3 currentTile;


    // Pheromones
    protected int pheromoneLeaveAmount = 5;


    // Colliders
    protected SphereCollider areaDetector;


    // Fight
    public int hp;
    protected int dmg;
    protected bool inFight = false;
    protected Coroutine attackCoroutine;
    protected float attackSpeed = 1.5f;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        areaDetector = GetComponentInChildren<SphereCollider>();
        chosenMoveIndex = Random.Range(0, 8);
    }


    // Update is called once per frame
    void Update()
    {

    }


    // Movement
    protected void Move()
    {
        Vector3 position = targetTile;
        position.y = Terrain.activeTerrain.SampleHeight(transform.position);
        agent.destination = position;
    }

    public void GoToPreviousTile()
    {
        //if (currentTile == targetTile)
        targetTile = previousTile;
        //else
        //targetTile = currentTile;
        chosenMoveIndex = Mathf.Abs(chosenMoveIndex - 7);
        Move();
    }

    protected void UpdateTargetTile()
    {
        if (surroundings[chosenMoveIndex] != null)
            targetTile = surroundings[chosenMoveIndex].transform.position;

        else
            targetTile = previousTile;
    }

    protected abstract void ChoseMoveIndex();

    protected int FindIndex(int?[] array, int value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null)
            {
                if (value <= array[i])
                    return i;
            }
        }
        // need to look around (-1 is special)
        return -1;
    }

    protected abstract void ChosenIndexValidation(int index);
    protected void GoToNextTile()
    {
        ChoseMoveIndex();
        UpdateTargetTile();
        Move();
    }


    // Owners
    public Owner GetOwner() => _owner;
    public void SetOwner(Owner owner) => _owner = owner;

    public AntsMaster GetAntsMaster() => _master;
    public void SetMaster(AntsMaster master) => _master = master;


    // Fight
    public int GetHp() => hp;
    public int GetDmg() => dmg;
    public bool IsInFight() => inFight;
    public void SetIsInFight(bool value) => inFight = value;
    public void DecreseHp(int value)
    {
        if (hp - value > 0)
            hp -= value;
        else
        {
            Debug.Log("Zero");
            hp = 0;
            Dead();
        }
    }

    public IEnumerator Attack(Ant target)
    {
        yield return new WaitForSeconds(attackSpeed);

        //Debug.Log("Dmg");
        if (target != null)
            target.DecreseHp(dmg);
        inFight = false;
        agent.isStopped = false;
        if (target != null && target.GetHp() > 0)
        {
            Fight(target);
        }
    }

    protected void Dead()
    {
        _master.DecreasePopulation();
        Destroy(gameObject);
    }
    
    public void Fight(Ant target)
    {
        if (!inFight)
        {
            //Debug.Log("InFight");
            inFight = true;
            agent.isStopped = true;
            
            attackCoroutine = StartCoroutine(Attack(target));
        }
    }
}
