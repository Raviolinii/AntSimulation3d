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
            else
            if (workerScript.WantToAlarm())
            {
                workerScript.StopAntNearDestination();
                workerScript.GoToPreviousTile();
                Alarm();
                if (!workerScript.WantToGather())
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
            dangerSpotted = true;
            antsMaster.Alarm();
        }
    }
}