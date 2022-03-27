using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAntNearFood : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AntWorker"))
        {
            WorkerAnt workerScript = other.GetComponentInParent<WorkerAnt>();
            workerScript.StopAntNearFood();
            workerScript.GatherFood();

        }
    }
}
