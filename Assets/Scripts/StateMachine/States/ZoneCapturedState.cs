using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCapturedState : BaseState
{
    private string zoneController;
    public ZoneCapturedState(ZoneStateMachine stateMachine) : base("ZoneCapturedState", stateMachine){}

    //entering with capture points to see who's got control of the zone
    public override void Enter(int ptsCaptureBluePassed, int ptsCaptureRedPassed, int nbBlueTankIn, int nbRedTankin)
    {
        base.Enter(ptsCaptureBluePassed, ptsCaptureRedPassed, nbBlueTankIn, nbRedTankin);
        ptsCaptureBlue = ptsCaptureBluePassed;
        ptsCaptureRed = ptsCaptureRedPassed;
        this.nbBlueTankIn = nbBlueTankIn;
        this.nbRedTankIn = nbRedTankin;
        

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

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log(currCooldown);
            Debug.Log(isCooldown);
        }

        /////// TRANSITIONS ///////
        ///
        // IF THE TEAM CONTROLLING LOSES CONTROL //
        if (ptsCaptureBlue <= 0 && zoneController == "blue")
        {
            ptsCaptureBlue = 0;
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneBaseState, ptsCaptureBlue, ptsCaptureRed, nbBlueTankIn, nbRedTankIn);
        }
        if(ptsCaptureRed <= 0 && zoneController == "red")
        {
            ptsCaptureRed = 0;
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneBaseState, ptsCaptureBlue, ptsCaptureRed, nbBlueTankIn, nbRedTankIn);
        }
        
        //IF CONTESTED //
        if(nbBlueTankIn > 0 && nbRedTankIn > 0)
        {
            //transi to contested
        }

        ///////// LOGIC ////////
        ///
        // BLUE TANKS ARE CAPTURING THE ZONE //
        if (nbBlueTankIn > 0 && nbRedTankIn == 0)
        {
            if (ptsCaptureRed == 0)
            {
                if (!isCooldown)
                {
                    //increasing cap points and activating cooldown
                    IncreasePtsCapBlue();
                    isCooldown = true;
                }
                else
                {
                   // CooldownTick();
                }
            }
            else
            {
                if (!isCooldown)
                {
                    DecreasePtsCapRed();
                    isCooldown = true;
                }
                else
                {
                   // CooldownTick();
                }
            }
        }

        // SLOWLY DECREASE POINTS WHEN NO TANK IS IN THE ZONE  //
        if (nbRedTankIn == 0 && nbBlueTankIn == 0)
        {
            if (ptsCaptureRed > 0)
            {
                if (!isCooldown)
                {
                    DecreasePtsCapRed();
                    isCooldown = true;
                }
                else
                {
                    LongCooldownTick();
                }
            }

            if (ptsCaptureBlue > 0)
            {
                if (!isCooldown)
                {
                    DecreasePtsCapBlue();
                    isCooldown = true;
                }
                else
                {
                    LongCooldownTick();
                }
            }
        }
    }
}