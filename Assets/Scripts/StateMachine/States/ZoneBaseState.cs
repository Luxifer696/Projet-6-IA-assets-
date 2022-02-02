using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneBaseState : BaseState
{
    private int _blueTeamCapturePts;

    public ZoneBaseState(ZoneStateMachine stateMachine) : base("ZoneBaseState", stateMachine){}

    public override void Enter()
    {
        base.Enter();
        _blueTeamCapturePts = 0;
    }

    public override void UpdateLogic()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _blueTeamCapturePts++;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            _blueTeamCapturePts--;
        }
        //transi to zone captured if points > 100
        if (_blueTeamCapturePts >= 100)
        {
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneCapturedState);
        }
    }
}
