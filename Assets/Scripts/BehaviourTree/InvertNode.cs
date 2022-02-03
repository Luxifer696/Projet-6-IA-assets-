using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertNode : DecoratorNode
{
    private bool response;
    protected override void OnStart()
    {
       
    }

    protected override void OnStop()
    {
       
    }

    protected override State OnUpdate()
    {
        if (response == false )
        {
            return State.RUNNING;
        }
        else
        {
            return State.FAILURE;
        }
        return State.SUCCESS;
    }

  
}
