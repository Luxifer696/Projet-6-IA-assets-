using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneClosAndNotCaptured : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
       
    }

    protected override State OnUpdate()
    {
        return State.RUNNING; 
    }

}
