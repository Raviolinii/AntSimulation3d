using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tile;

public class WorldObject : MonoBehaviour
{
    public Tile _tile;
    protected SphereCollider stoppingDistance;
    public SpawnedObject typeOfObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
