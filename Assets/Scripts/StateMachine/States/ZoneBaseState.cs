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
        ptsCaptureBlue -= nbRedTankIn;
    }

    private void DecreasePtsCapRed()
    {
        ptsCaptureRed -= nbBlueTankIn;
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
    
    public override void UpdateLogic()
    {
        
        if (Input.GetKey(KeyCode.A))
        {
            
        }

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
        
        if (nbRedTankIn > 0 && nbBlueTankIn == 0)
        {
            if (ptsCaptureBlue == 0)
            {
                if (!isCooldown)
                {
                    //increasing cap points and activating cooldown
                    IncreasePtsCapRed();
                    isCooldown = true;
                    Debug.Log("pts red : " + ptsCaptureRed);
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
                    Debug.Log("pts blue : " + ptsCaptureBlue);
                }
                else
                {
                    CooldownTick();
                }
            }
        }
    }
}
