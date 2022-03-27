using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    object _ = new object();
    int amount = 2_000;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetFoodAmount() => amount;
    public int GatherFood(int value)
    {
        int decresedBy = 0;
        lock (_)
        {
            if (amount < value)
            {
                decresedBy = amount;
                amount = 0;
                Depleted();
            }
            else
            {
                decresedBy = value;
                amount -= value;
            }
        }
        return decresedBy;
    }
    void Depleted()
    {
        Destroy(gameObject);
    }
}
