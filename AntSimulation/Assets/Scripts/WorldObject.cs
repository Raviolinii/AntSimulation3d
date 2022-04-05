using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    protected Tile _tile;
    protected SphereCollider stoppingDistance;
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
            Debug.Log("Tile detected");
            _tile = other.GetComponent<Tile>();
            _tile.ObjectSpawned();
            Debug.Log(_tile);
        }
    }
    protected virtual void OnDestroy()
    {
        _tile.ObjectDestroyed();
    }

    public void SetTile(Tile tile) => _tile = tile;
    public Tile GetTile() => _tile;
}
