using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ZoneBaseState : BaseState
{

    public ZoneBaseState(ZoneStateMachine stateMachine) : base("ZoneBaseState", stateMachine){}

    public override void Enter(int ptsCaptureBluePassed, int ptsCaptureRedPassed, int nbBlueTankIn, int nbRedTankIn)
    {
        base.Enter(ptsCaptureBluePassed, ptsCaptureRedPassed, nbBlueTankIn, nbRedTankIn);
    }

    public override int GetNbPointBlue()
    {
        return ptsCaptureBlue;
    }
    
    public override int GetNbPointRed()
    {
        return ptsCaptureRed;
    }
    
    public override void UpdateLogic()
    {
        
        if (Input.GetKey(KeyCode.A))
        {
            
        }

        // TRANSITION TO CAPTURED STATE IF A TEAM REACHED MAX PTS //
        //transi to zone captured if points > 100
        if (ptsCaptureBlue == maxPtsForCap)
        {
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneCapturedState, ptsCaptureBlue, ptsCaptureRed, nbBlueTankIn, nbRedTankIn);
        }

        if (ptsCaptureRed == maxPtsForCap)
        {
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneCapturedState, ptsCaptureBlue, ptsCaptureRed, nbBlueTankIn, nbRedTankIn);
        }
        
        // TRANSITION TO CONTESTED STATE IF BOTH TEAM ARE IN THE ZONE //
        if (nbBlueTankIn != 0 && nbRedTankIn != 0)
        {
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneContestedState, ptsCaptureBlue, ptsCaptureRed, nbBlueTankIn, nbRedTankIn);
        }
        
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
                    CooldownTick();
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
                    CooldownTick();
                }
            }
        }
        
        // RED TANKS ARE CAPTURING THE ZONE //
        if (nbRedTankIn > 0 && nbBlueTankIn == 0)
        {
            if (ptsCaptureBlue == 0)
            {
                if (!isCooldown)
                {
                    //increasing cap points and activating cooldown
                    IncreasePtsCapRed();
                    isCooldown = true;
                } 
                else
                {
                    CooldownTick();
                }
            }
            else
            {
                if (!isCooldown)
                {
                    DecreasePtsCapBlue();
                    isCooldown = true;
                }
                else
                {
                    CooldownTick();
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
