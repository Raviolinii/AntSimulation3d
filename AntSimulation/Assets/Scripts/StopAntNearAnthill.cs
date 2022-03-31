using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAntNearAnthill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AntWorker"))
        {
            WorkerAnt workerScript = other.GetComponentInParent<WorkerAnt>();
            if (workerScript.WantToStoreFood())
            {
                workerScript.StopAntNearDestination();
                workerScript.StoreFood();
            }

        }
    }
}
