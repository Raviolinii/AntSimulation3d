using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PheromoneMap : MonoBehaviour
{
    int width = 3; // 50 50
    int height = 3;
    public GameObject tile;
    Tile[,] tileMap;    // changed from pheromones

    float pheromonesDecreseTime = 2f;
    int pheromoneDecreseValue = 1;

    public GameObject foodPrefab;
    public GameObject anthillPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CreateTileMap();
        AsignSurroundings();

        InvokeRepeating("DecreasePheromones", pheromonesDecreseTime, pheromonesDecreseTime);

        Invoke("TestSpawnFood", 1.5f);
        //Invoke("TestSpawnAnthill", 1.5f);
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    void TestSpawnFood() => SpawnFoodAtIndex(1, 1);
    void TestSpawnAnthill() => SpawnAnthillAtIndex(Owner.player, 2, 2);
    /* void TestSpawnFood()
    {
        SpawnFoodAtIndex(1,1);
        SpawnFoodAtIndex(1,2);
        SpawnFoodAtIndex(2,1);
        SpawnFoodAtIndex(2,2);
    } */
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
        //position.y = Terrain.activeTerrain.SampleHeight(transform.position);  // Test with terrain required
        position.y = 2;

        Instantiate(anthillPrefab, position, anthillPrefab.transform.rotation);
    }

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
                currentGameObject = Instantiate(tile, new Vector3(j * 5f, -2, -i * 5f), tile.transform.rotation);
                currentScript = currentGameObject.GetComponent<Tile>();
                tileMap[i, j] = currentScript;
                //pheromonesMap[i, j].SetIndex(i, j);
            }
        }
    }

    void DecreasePheromones()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                tileMap[i, j].DecreasePheromones(pheromoneDecreseValue);
            }
        }
    }
}
