using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anthill : WorldObject
{
    // Owner
    public Owner _owner;
    public AntsMaster antsMaster;
    //public GameObject queen;

    // Alarm
    bool dangerSpotted = false;

    // Tiles
    public Tile[] surroundings = new Tile[16];

    // Fight
    int hp = 100;
    int dmg = 2;
    bool inFight = false;
    Coroutine attackCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        stoppingDistance = GetComponentInChildren<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }


    // Owner
    public void SetMaster(AntsMaster master) => antsMaster = master;
    public void SetOwner(Owner owner) => _owner = owner;
    public Owner GetOwner() => _owner;


    // Food
    public bool AddFood(int value) => antsMaster.AddFood(value);


    // Alarm
    public void Alarm()
    {
        if (!dangerSpotted)
        {
            dangerSpotted = true;
            antsMaster.Alarm();
        }
    }


    // Destroy
    protected void OnDestroy()
    {
        Tile[] otherParts = _tile.GetActualSurroundings();
        for (int i = 0; i < 8; i++)
        {
            otherParts[i].ObjectDestroyed();
        }
        _tile.ObjectDestroyed();
    }


    // Tiles
    public override void SetTile(Tile tile)
    {
        base.SetTile(tile);
        AsignSurroundings();
    }

    private void AsignSurroundings()
    {
        Tile current = _tile.GetTile(0);
        surroundings[0] = current.GetTile(0);
        surroundings[1] = current.GetTile(1);
        surroundings[2] = current.GetTile(2);
        surroundings[5] = current.GetTile(3);
        surroundings[7] = current.GetTile(5);

        current = _tile.GetTile(2);
        surroundings[3] = current.GetTile(1);
        surroundings[4] = current.GetTile(2);
        surroundings[6] = current.GetTile(4);
        surroundings[8] = current.GetTile(7);

        current = _tile.GetTile(5);
        surroundings[9] = current.GetTile(3);
        surroundings[11] = current.GetTile(5);
        surroundings[12] = current.GetTile(6);

        current = _tile.GetTile(7);
        surroundings[10] = current.GetTile(4);
        surroundings[13] = current.GetTile(5);
        surroundings[14] = current.GetTile(6);
        surroundings[15] = current.GetTile(7);
    }
    public Tile[] GetSurroundings() => surroundings;

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
            QuinDied();
        }
    }

    private IEnumerator Attack(Ant target)
    {
        yield return new WaitForSeconds(1.5f);

        //Debug.Log("Dmg");
        if (target != null)
            target.DecreseHp(dmg);
        inFight = false;
        if (target != null && target.GetHp() > 0)
        {
            Fight(target);
        }
    }
    protected void QuinDied()
    {
        Debug.Log($"GG, {_owner} lost");
        antsMaster.QuinDied();
    }
    
    public void Fight(Ant target)
    {
        if (!inFight)
        {
            //Debug.Log("InFight");
            inFight = true;
            
            attackCoroutine = StartCoroutine(Attack(target));
        }
    }
}