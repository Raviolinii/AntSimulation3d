using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    int width = 40;
    int height = 40;
    int[,] map; 

    // Start is called before the first frame update
    void Start()
    {
        map = new int[height, width];
    }

    // Position
    public Vector2Int RandomPosition(int objectType)
    {
        Vector2Int result = new Vector2Int();
        result.x = Random.Range(1, height);
        result.y = Random.Range(1, width);

        if (!PositionValidation(result.x, result.y))
            result = RandomPosition(objectType);

        map[result.x, result.y] = objectType;

        return result;
    }

    bool PositionValidation(int i, int j)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (map[i + x, j + y] != 0)
                    return false;
            }
        }
        return true;
    }

    // Parameters
    public int GetWidht() => width;
    public int GetHeight() => height;
}
