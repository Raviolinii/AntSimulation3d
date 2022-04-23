using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    // Parameters
    int width = 50;         // 40??
    int height = 50;        // 40??
    int treesCount = 20;

    // Map
    int[,] map;
    byte[] anthillsInQuarters = new byte[4];


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Random.Range(0, 2));
        Debug.Log(Random.Range(0, 2));
        Debug.Log(Random.Range(0, 2));
        Debug.Log(Random.Range(0, 2));
        Debug.Log(Random.Range(0, 2));
        Debug.Log(Random.Range(0, 2));
        map = new int[height, width];
    }

    // Quarters
    int RandomQuarter() => Random.Range(0, 4);

    int RandomQuarterAnthill()
    {
        int quarter = RandomQuarter();
        if (anthillsInQuarters[quarter] != 0)
            quarter = RandomQuarterAnthill();

        return quarter;
    }


    // Position
    public Vector2Int RandomPosition(int objectType)
    {
        int quarter = RandomQuarter();
        Vector2Int result = RandomPositionForObject(quarter);
        if (!PositionValidation(result.x, result.y))
            result = RandomPosition(objectType);

        map[result.x, result.y] = objectType;

        return result;
    }

    public Vector2Int RandomAnthillPosition()
    {
        int quarter = RandomQuarterAnthill();
        anthillsInQuarters[quarter] = 1;

        Vector2Int result = ValidatedRandomAnthillPosition(quarter);

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                map[result.x + x, result.y + y] = 1;
            }
        }

        return result;
    }


    // Points
    Vector2Int RandomPositionForObject(int quarter)
    {
        Vector2Int result = new Vector2Int();

        switch (quarter)
        {
            case 0:
                result.x = Random.Range(1, height / 2 - 1);
                result.y = Random.Range(1, width / 2 - 1);
                break;

            case 1:
                result.x = Random.Range(1, height / 2 - 1);
                result.y = Random.Range(width / 2 + 1, width - 1);
                break;

            case 2:
                result.x = Random.Range(height / 2 + 1, height - 1);
                result.y = Random.Range(1, width / 2 - 1);
                break;

            case 3:
                result.x = Random.Range(height / 2 + 1, height - 1);
                result.y = Random.Range(width / 2 + 1, width - 1);
                break;
        }

        return result;
    }

    Vector2Int ValidatedRandomAnthillPosition(int quarter)
    {
        Vector2Int result = new Vector2Int();

        switch (quarter)
        {
            case 0:
                result.x = Random.Range(2, height / 2 - 2);
                result.y = Random.Range(2, width / 2 - 2);
                break;

            case 1:
                result.x = Random.Range(2, height / 2 - 2);
                result.y = Random.Range(width / 2 + 2, width - 2);
                break;

            case 2:
                result.x = Random.Range(height / 2 + 2, height - 2);
                result.y = Random.Range(2, width / 2 - 2);
                break;

            case 3:
                result.x = Random.Range(height / 2 + 2, height - 2);
                result.y = Random.Range(width / 2 + 2, width - 2);
                break;
        }

        if (!AnthillPositionValidation(result.x, result.y))
            result = ValidatedRandomAnthillPosition(quarter);

        return result;
    }


    // Validation
    bool AnthillPositionValidation(int i, int j)
    {
        for (int x = -2; x < 3; x++)
        {
            for (int y = -2; y < 3; y++)
            {
                if (map[i + x, j + y] != 0)
                    return false;
            }
        }
        return true;
    }

    bool PositionValidation(int i, int j)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                Debug.Log($"j: {i}, x: {x}, i: {j}, y: {y}");
                if (map[i + x, j + y] != 0)
                    return false;
            }
        }
        return true;
    }


    // Parameters
    public int GetWidht() => width;
    public int GetHeight() => height;
    public int GetTreesCount() => treesCount;
}
