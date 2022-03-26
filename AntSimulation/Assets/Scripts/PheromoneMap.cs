using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PheromoneMap : MonoBehaviour
{
    int width = 50;
    int height = 50;
    public GameObject pheromone;
    Pheromone[,] pheromonesMap;

    float pheromonesDecreseTime = 2f;
    int pheromoneDecreseValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        CreatePheromonesMap();
        AsignSurroundings();

        InvokeRepeating("DecreasePheromones", pheromonesDecreseTime, pheromonesDecreseTime);

    }

    // Update is called once per frame
    void Update()
    {

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
            pheromonesMap[i, j].AddToSurroundings(pheromonesMap[i + iOffset, j + jOffset], index);
        }
        catch (System.IndexOutOfRangeException)
        {
            return;
        }
    }

    private void CreatePheromonesMap()
    {
        pheromonesMap = new Pheromone[height, width];
        GameObject currentGameObject;
        Pheromone currentScript;


        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                currentGameObject = Instantiate(pheromone, new Vector3(j * 5f, -2, -i * 5f), pheromone.transform.rotation);
                currentScript = currentGameObject.GetComponent<Pheromone>();
                pheromonesMap[i, j] = currentScript;
                pheromonesMap[i, j].SetIndex(i, j);
            }
        }
    }

    void DecreasePheromones()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                pheromonesMap[i, j].DecreasePheromones(pheromoneDecreseValue);
            }
        }
    }
}
