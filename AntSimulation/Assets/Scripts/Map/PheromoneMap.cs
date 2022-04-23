using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PheromoneMap : MonoBehaviour
{
    // Map
    public GameObject tile;
    int width;
    int height;
    Tile[,] tileMap;    // changed from pheromones
    MapInfo mapInfo;

    // Pheromones
    float pheromonesDecreseTime = 10f;
    int pheromoneDecreseValue = 2;          // Move it to pheromone script

    // World Objects
    public GameObject foodPrefab;
    public GameObject anthillPrefab;
    public GameObject treePrefab;


    // Start is called before the first frame update
    void Start()
    {
        mapInfo = GetComponent<MapInfo>();
        width = mapInfo.GetWidht();
        height = mapInfo.GetHeight();
        
        CreateTileMap();
        AsignSurroundings();

        InvokeRepeating("DecreasePheromones", pheromonesDecreseTime, pheromonesDecreseTime);

        Invoke("TestSpawnFood", 1.5f);
        Invoke("TestSpawnPlayersAnthill", 1.5f);
        Invoke("TestSpawnAiAnthill", 1.5f);
        Invoke("TestSpawnTrees", 1.5f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // World Objects
    void TestSpawnFood() => SpawnFoodAtIndex(1, 1);
    void TestSpawnPlayersAnthill() => SpawnAnthillAtIndex(Owner.player, 4, 4);
    void TestSpawnAiAnthill() => SpawnAnthillAtIndex(Owner.AI, 20, 20);
    void TestSpawnTrees()
    {
        SpawnTreeAtIndex(0,0);
        SpawnTreeAtIndex(10,6);
        SpawnTreeAtIndex(8,14);
        SpawnTreeAtIndex(14,15);
        SpawnTreeAtIndex(17,24);
    }
    void SpawnFoodAtIndex(int i, int j)
    {
        Vector3 position = tileMap[i, j].transform.position;
        position.y = 10;
        Instantiate(foodPrefab, position, foodPrefab.transform.rotation);
    }

    void SpawnAnthillAtIndex(Owner owner, int i, int j)
    {
        Anthill prefabScript = anthillPrefab.GetComponent<Anthill>();
        prefabScript._owner = owner;

        Vector3 position = tileMap[i, j].transform.position;
        position.y = Terrain.activeTerrain.SampleHeight(position);

        Anthill spawned = Instantiate(anthillPrefab, position, anthillPrefab.transform.rotation).GetComponent<Anthill>();
        spawned.SetTile(tileMap[i, j]);
    }

    void SpawnTreeAtIndex(int i, int j)
    {
        Vector3 position = tileMap[i, j].transform.position;
        position.y = Terrain.activeTerrain.SampleHeight(position);

        Instantiate(treePrefab, position, treePrefab.transform.rotation);
    }

    // Map
    void AsignSurroundings()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                AddSurroundingsFor(i, j);
            }
        }
    }

    void AddSurroundingsFor(int i, int j)
    {
        TryAddOneOfSurroundings(i, -1, j, -1, 0);
        TryAddOneOfSurroundings(i, -1, j, 0, 1);
        TryAddOneOfSurroundings(i, -1, j, 1, 2);

        TryAddOneOfSurroundings(i, 0, j, -1, 3);
        TryAddOneOfSurroundings(i, 0, j, 1, 4);

        TryAddOneOfSurroundings(i, 1, j, -1, 5);
        TryAddOneOfSurroundings(i, 1, j, 0, 6);
        TryAddOneOfSurroundings(i, 1, j, 1, 7);
    }

    void TryAddOneOfSurroundings(int i, int iOffset, int j, int jOffset, int index)
    {
        try
        {
            tileMap[i, j].AddToSurroundings(tileMap[i + iOffset, j + jOffset], index);
        }
        catch (System.IndexOutOfRangeException)
        {
            return;
        }
    }

    private void CreateTileMap()
    {
        tileMap = new Tile[height, width];
        GameObject currentGameObject;
        Tile currentScript;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                currentGameObject = Instantiate(tile, new Vector3(j * 5f, 1, -i * 5f), tile.transform.rotation);
                currentScript = currentGameObject.GetComponent<Tile>();
                tileMap[i, j] = currentScript;
            }
        }
    }

    void DecreasePheromones()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //Debug.Log(tileMap[i,j].GetWorkerPheromoneValue());
                tileMap[i, j].DecreasePheromones(pheromoneDecreseValue);
            }
        }
    }
}