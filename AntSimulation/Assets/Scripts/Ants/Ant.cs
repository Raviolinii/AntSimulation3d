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
    public Vector3 targetTile;
    public Tile[] surroundings = new Tile[8];
    protected Tile tileScript;
    public Vector3 previousTile;
    public Vector3 currentTile;


    // Pheromones
    protected int pheromoneLeaveAmount = 7;
    protected int pheromoneMultiply = 3;


    // Colliders
    protected SphereCollider areaDetector;


    // Fight
    public int hp;
    protected int dmg;
    protected bool inFight = false;
    protected Coroutine attackCoroutine;
    protected float attackSpeed = 3f;


    // Death 
    protected bool isDead = false;
    protected Coroutine dyigCoroutine;
    protected float dyingTime = 3f;


    // Animations
    protected Animator animator;
    protected const string isAttacking = "IsAttacking";
    protected const string isDying = "IsDying";
    protected const string isMoving = "IsMoving";


    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        animator = GetComponent<Animator>();
        areaDetector = GetComponentInChildren<SphereCollider>();
        chosenMoveIndex = Random.Range(0, 8);
    }


    // Update is called once per frame
    void Update()
    {
        //animator.SetBool(isMoving, agent.velocity.magnitude > 0.01f);
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
            hp = 0;
            Dead();
        }
    }

    public IEnumerator Attack(Ant target)
    {
        animator.SetBool(isAttacking, true);
        yield return new WaitForSeconds(attackSpeed);
        animator.SetBool(isAttacking, false);

        inFight = false;
        if (target != null)
        {
            target.DecreseHp(dmg);

            if (target.GetHp() > 0)
                Fight(target);
        }
        else
        {
            agent.isStopped = false;
            animator.SetBool(isMoving, true);
        }

    }


    public void Fight(Ant target)
    {
        if (!inFight)
        {
            inFight = true;
            agent.isStopped = true;
            animator.SetBool(isMoving, false);

            attackCoroutine = StartCoroutine(Attack(target));
        }
    }


    // Death
    public void Dead()
    {
        if (dyigCoroutine == null)
            dyigCoroutine = StartCoroutine(DyingHandler());
    }

    public IEnumerator DyingHandler()
    {
        _master.DecreasePopulation();
        animator.SetBool(isDying, true);
        yield return new WaitForSeconds(dyingTime);
        animator.SetBool(isDying, false);

        Destroy(gameObject);
    }

    public bool GetIsDead() => isDead;
}