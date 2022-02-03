using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isInZoneNode : DecoratorNode
{
    public bool response;
    public Blackboard blackboard;
    private bool _bIsInZone; 
    
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        Vector3 ZonePosition = blackboard.PSPosition;
        Vector3 TankPosition = blackboard.TankPosition;
        Vector3 DistanceBetweenTankAndZone = TankPosition;

        ZonePosition.x = 2.57f;
        ZonePosition.y = -0.35f;
        ZonePosition.z = 1.58f;

        //récupérer la position du tank

        if ( response == false && _bIsInZone == false)
        {
            return State.FAILURE; 
        }else
        {
            return State.RUNNING; 
        }
        return State.SUCCESS; 
    }

  
}
