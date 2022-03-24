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

    // Start is called before the first frame update
    void Start()
    {
        CreatePheromonesMap();
        //AsignSurroundings();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void AsignSurroundings()
    {
        for (int i = 1; i < height - 1; i++)
        {
            for (int j = 1; j < width - 1; j++)
            {
                AddSurroundingsFor(i, j);
            }
        }
    }
    void AddSurroundingsFor(int i, int j)
    {
        pheromonesMap[i, j].AddToSurroundings(pheromonesMap[i - 1, j - 1], 0);
        pheromonesMap[i, j].AddToSurroundings(pheromonesMap[i - 1, j], 1);
        pheromonesMap[i, j].AddToSurroundings(pheromonesMap[i - 1, j + 1], 2);

        pheromonesMap[i, j].AddToSurroundings(pheromonesMap[i, j - 1], 3);
        pheromonesMap[i, j].AddToSurroundings(pheromonesMap[i, j + 1], 4);

        pheromonesMap[i, j].AddToSurroundings(pheromonesMap[i + 1, j - 1], 5);
        pheromonesMap[i, j].AddToSurroundings(pheromonesMap[i + 1, j], 6);
        pheromonesMap[i, j].AddToSurroundings(pheromonesMap[i + 1, j + 1], 7);
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
                currentGameObject = Instantiate(pheromone, new Vector3(j * 5f, 1, -i * 5f), pheromone.transform.rotation);
                currentScript = currentGameObject.GetComponent<Pheromone>();
                pheromonesMap[i, j] = currentScript;
                pheromonesMap[i, j].SetIndex(i, j);
            }
        }
    }

}
