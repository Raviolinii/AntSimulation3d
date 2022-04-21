using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anthill : WorldObject
{
    // Owner
    public Owner _owner;
    protected AntsMaster antsMaster;
    //public GameObject queen;

    // Alarm
    bool dangerSpotted = false;

    // Tiles
    public Tile[] surroundings = new Tile[16];

    // Start is called before the first frame update
    void Start()
    {
        stoppingDistance = GetComponentInChildren<SphereCollider>();
        typeOfObject = SpawnedObject.anthill;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("AntWorker"))
        {
            WorkerAnt workerScript = other.GetComponentInParent<WorkerAnt>();

            if (!workerScript.WantToAlarm() && !workerScript.WantToStoreFood())
                return;

            if (workerScript.WantToAlarm())
            {
                Debug.Log("Wants to alarm");
                workerScript.StopAntNearDestination();
                workerScript.GoToPreviousTile();
                workerScript.SetAnthillScript(this);
                workerScript.RaiseAlarm();
            }

            if (workerScript.WantToStoreFood())
            {
                workerScript.StopAntNearDestination();
                workerScript.GoToPreviousTile();
                workerScript.StoreFood();
            }
        }
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
            Debug.Log("Alarm from anthill");
            dangerSpotted = true;
            antsMaster.Alarm();
        }
    }


    // Destroy
    protected override void OnDestroy()
    {
        Tile[] otherParts = _tile.GetActualSurroundings();
        for (int i = 0; i < 8; i++)
        {
            otherParts[i].ObjectDestroyed();
        }
        base.OnDestroy();
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
}