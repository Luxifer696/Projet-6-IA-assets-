using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : NodeBT
{

    public NodeBT child; 
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return child.Update(); 
    }

}
