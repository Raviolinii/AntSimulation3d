using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anthill : WorldObject
{
    public Owner _owner;
    protected AntsMaster antsMaster;
    //public GameObject queen;

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
            if (workerScript.WantToStoreFood())
            {
                workerScript.StopAntNearDestination();
                workerScript.GoToPreviousTile();
                workerScript.StoreFood();
            }
        }
    }

    public void SetMaster(AntsMaster master) => antsMaster = master; 
    public bool AddFood(int value) => antsMaster.AddFood(value);
    public void SetOwner(Owner owner) => _owner = owner;
    public Owner GetOwner() => _owner;
}