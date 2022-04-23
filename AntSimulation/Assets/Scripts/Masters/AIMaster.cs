using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMaster : AntsMaster
{
    // Start is called before the first frame update
    protected override void Start()
    {
        owner = Owner.AI;
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
