using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ZoneBaseState : BaseState
{

    public ZoneBaseState(ZoneStateMachine stateMachine) : base("ZoneBaseState", stateMachine){}
    
    public int nbBlueTankIn = 0;
    public int nbRedTankIn = 0;
    public int ptsCaptureBlue = 0;
    public int ptsCaptureRed = 0;
    private float ptsCooldown = 1f;
    private float ptsLongCooldown = 3f; // cooldown used for slow decrease
    private float currCooldown;
    private bool isCooldown = false;

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
    }
    
    private void IncreasePtsCapRed()
    {
        ptsCaptureRed += nbRedTankIn;
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
                    Debug.Log("pts blue : " + ptsCaptureBlue);
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
                    Debug.Log("pts red : " + ptsCaptureRed);
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
                    Debug.Log("pts red slowly dec");
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
                    Debug.Log("pts blue slowly dec");
                }
                else
                {
                    LongCooldownTick();
                }
            }
        }
    }
}
