using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ZoneBaseState : BaseState
{

    public ZoneBaseState(ZoneStateMachine stateMachine) : base("ZoneBaseState", stateMachine){}
    
    public int nbBlueTankIn = 0;
    public int nbRedTankIn = 0;
    private float ptsCooldown = 1f;
    private float ptsLongCooldown = 3f; // cooldown used for slow decrease
    private float currCooldown;
    private bool isCooldown = false;
    private int maxPtsForCap = 15;

    public override void Enter()
    {
        base.Enter();
    }

    public override int GetNbPointBlue()
    {
        return ptsCaptureBlue;
    }
    
    public override int GetNbPointRed()
    {
        return ptsCaptureRed;
    }

    private void IncreasePtsCapBlue()
    {
        ptsCaptureBlue += nbBlueTankIn;
        
        //make sure we dont go above points for cap
        if (ptsCaptureBlue > maxPtsForCap)
        {
            ptsCaptureBlue = maxPtsForCap;
        }
    }
    
    private void IncreasePtsCapRed()
    {
        ptsCaptureRed += nbRedTankIn;
        
        //make sure we dont go above points for cap
        if (ptsCaptureRed > maxPtsForCap)
        {
            ptsCaptureRed = maxPtsForCap;
        }
    }

    private void DecreasePtsCapBlue()
    {
        ptsCaptureBlue -= nbRedTankIn + 1; //decrease by number of enemy tanks + 1
        //make sure we dont go below 0
        if (ptsCaptureBlue < 0)
        {
            ptsCaptureBlue = 0;
        }
    }

    private void DecreasePtsCapRed()
    {
        ptsCaptureRed -= nbBlueTankIn + 1; //decrease by number of enemy tanks + 1
        //make sure we dont go below 0
        if (ptsCaptureRed < 0)
        {
            ptsCaptureRed = 0;
        }
    }

    private void CooldownTick()
    {
        currCooldown += Time.deltaTime;
        if (currCooldown >= ptsCooldown)
        {
            isCooldown = false;
            currCooldown = 0f;
        }
    }
    
    private void LongCooldownTick()
    {
        currCooldown += Time.deltaTime;
        if (currCooldown >= ptsLongCooldown)
        {
            isCooldown = false;
            currCooldown = 0f;
        }
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
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneCapturedState, ptsCaptureBlue, ptsCaptureRed);
        }

        if (ptsCaptureRed == maxPtsForCap)
        {
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneCapturedState, ptsCaptureBlue, ptsCaptureRed);
        }
        
        // TRANSITION TO CONTESTED STATE IF BOTH TEAM ARE IN THE ZONE //
        if (nbBlueTankIn != 0 && nbRedTankIn != 0)
        {
            stateMachine.ChangeState(((ZoneStateMachine)stateMachine).zoneContestedState, ptsCaptureBlue, ptsCaptureRed);
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
