using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Pheromone workerPheromone = new Pheromone();
    Pheromone workerFoodPheromone = new Pheromone();
    Pheromone warriorPheromone = new Pheromone();
    public WorldObject objectOnTile;
    public SpawnedObject spawnedObjectType;
    public Tile[] surroundings = new Tile[8];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToSurroundings(Tile tile, int index) => surroundings[index] = tile;
    public int GetWorkerPheromoneValue() => workerPheromone.GetPheromoneValue();
    public int GetWorkerFoodPheromoneValue() => workerFoodPheromone.GetPheromoneValue();
    public int GetWarriorPheromoneValue() => warriorPheromone.GetPheromoneValue();

    public void AddWorkerPheromone(int value) => workerPheromone.AddPheromone(value);
    public void AddWorkerFoodPheromone(int value) => workerFoodPheromone.AddPheromone(value);
    public void AddWarriorPheromone(int value) => warriorPheromone.AddPheromone(value);

    public void DecreasePheromones(int value)
    {
        workerFoodPheromone.DecreasePheromone(value);
        workerPheromone.DecreasePheromone(value);
        warriorPheromone.DecreasePheromone(value);
    }
    public Tile[] GetSurroundings() => (Tile[])surroundings.Clone();
    public Tile[] GetSurroundingsNulls(int chosenMoveIndex)
    {
        Tile[] result = (Tile[])surroundings.Clone();
        for (int i = 0; i < 8; i++)
        {            
            if (result[i] != null && result[i].GetSpawnedObjectType() == SpawnedObject.obstacle)
                result[i] = null;
        }

        switch (chosenMoveIndex)
        {
            case 0:
                {
                    result[4] = null;
                    result[6] = null;
                    result[7] = null;
                    break;
                }
            case 1:
                {
                    result[5] = null;
                    result[6] = null;
                    result[7] = null;
                    break;
                }
            case 2:
                {
                    result[3] = null;
                    result[5] = null;
                    result[6] = null;
                    break;
                }
            case 3:
                {
                    result[2] = null;
                    result[4] = null;
                    result[7] = null;
                    break;
                }
            case 4:
                {
                    result[0] = null;
                    result[3] = null;
                    result[5] = null;
                    break;
                }
            case 5:
                {
                    result[1] = null;
                    result[2] = null;
                    result[4] = null;
                    break;
                }
            case 6:
                {
                    result[0] = null;
                    result[1] = null;
                    result[2] = null;
                    break;
                }
            case 7:
                {
                    result[0] = null;
                    result[1] = null;
                    result[3] = null;
                    break;
                }

            default:
                throw new System.Exception("Invalid chosenMoveIndex in GetSurroundings method");
        }
        return result;
    }

    public Tile GetTile(int index) => surroundings[index];
    public SpawnedObject GetSpawnedObjectType() => spawnedObjectType;
    public WorldObject GetSpawnedObject() => objectOnTile;
    public void ObjectSpawned(WorldObject objectSpawned, SpawnedObject spawned)
    {
        objectOnTile = objectSpawned;
        spawnedObjectType = spawned;
    }
    public void ObjectDestroyed()
    {
        objectOnTile = null;
        spawnedObjectType = SpawnedObject.no;
    }
}
