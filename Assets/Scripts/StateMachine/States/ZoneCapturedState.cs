using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCapturedState : BaseState
{
    private int _blueTeamCapturePts = 15;

    public ZoneCapturedState(ZoneStateMachine stateMachine) : base("ZoneCapturedState", stateMachine){}

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        //transi to zone base state if points = 0
        if (_blueTeamCapturePts <= 0)
        { 
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneBaseState);
        }

    }
}