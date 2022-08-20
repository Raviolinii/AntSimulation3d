using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tile;

public class WorldObject : MonoBehaviour
{
    public Tile _tile;
    protected SphereCollider stoppingDistance;
    public SpawnedObject typeOfObject;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            Tile tileScript = other.GetComponent<Tile>();
            tileScript.ObjectSpawned(this, typeOfObject);
            SetTile(tileScript);
        }
    }

    public virtual void SetTile(Tile tile) => _tile = tile;
    public Tile GetTile() => _tile;
}
