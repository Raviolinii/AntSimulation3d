using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersMaster : AntsMaster
{
    // Start is called before the first frame update
    protected override void Start()
    {
        owner = Owner.player;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
