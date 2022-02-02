using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneContestedState : BaseState
{

    public ZoneContestedState(ZoneStateMachine stateMachine) : base("ZoneContestedState", stateMachine){}

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        
        
        //transi to zone base state if points = 0
        //if ( tanks = none ==> go back to other state )
        //{ 
        //    stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneBaseState);
        //}
    }
}