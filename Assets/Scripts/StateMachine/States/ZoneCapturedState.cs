using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCapturedState : BaseState
{
    private string zoneController;
    private bool boole = false;
    public ZoneCapturedState(ZoneStateMachine stateMachine) : base("ZoneCapturedState", stateMachine){}

    //entering with capture points to see who's got control of the zone
    public override void Enter(int ptsCaptureBlueIn, int ptsCaptureRedIn)
    {
        base.Enter(ptsCaptureBlueIn, ptsCaptureRedIn);
        ptsCaptureBlue = ptsCaptureBlueIn;
        ptsCaptureRed = ptsCaptureRedIn;

        // get who's controlling
        if(ptsCaptureBlue > ptsCaptureRed)
        {
            zoneController = "blue";
        } else
        {
            zoneController = "red";
        }
    }



    public override void UpdateLogic()
    {
        //CONTROLLER POINT == 0 ==> TRANSI BACK TO NO CONTROL STATE //
        if (ptsCaptureBlue <= 0 && zoneController == "blue")
        {
            ptsCaptureBlue = 0;
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneBaseState, ptsCaptureBlue, ptsCaptureRed);
        }
        if(ptsCaptureRed <= 0 && zoneController == "red")
        {
            ptsCaptureRed = 0;
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneBaseState, ptsCaptureBlue, ptsCaptureRed);
        }
        if (!boole)
        {
            Debug.Log("pts blue : " + ptsCaptureBlue);
            Debug.Log("pts red : " + ptsCaptureRed);
            boole = true;
        }


    }
}