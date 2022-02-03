using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string name;
    protected StateMachine stateMachine;
    public int nbBlueTankIn;
    public int nbRedTankIn;
    public int ptsCaptureBlue;
    public int ptsCaptureRed;
    public static int maxPtsForCap = 15;
    public float ptsCooldown = 1f;
    public float ptsLongCooldown = 3f; // cooldown used for slow decrease
    public float currCooldown = 0f;
    public bool isCooldown = false;

    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual int GetNbPointBlue()
    {
        return 0;
    }
    
    public virtual int GetNbPointRed()
    {
        return 0;
    }
    
    public virtual void Enter() { }

    //overloading enter to pass arguments
    //used for captured state
    public virtual void Enter(int ptsCaptureBlue, int ptsCaptureRed, int nbTankBlueIn, int nbTankRedIn) { }
    //used for contested state ?
    //public virtual void Enter(int ptsCaptureBlue, int ptsCaptureRed) { }
    // USE THIS TO GO BACK TO BASE STATE WITH TANKSIN
    //public virtual void Enter(int nbTankBlueIn, int nbTankRedIn) {}?????
    
    public virtual void Exit() { }

    public virtual void UpdateLogic() { }

    public virtual void UpdatePhysics() { }

    public void IncreasePtsCapBlue()
    {
        ptsCaptureBlue += nbBlueTankIn;

        //make sure we dont go above points for cap
        if (ptsCaptureBlue > maxPtsForCap)
        {
            ptsCaptureBlue = maxPtsForCap;
        }
    }

    public void IncreasePtsCapRed()
    {
        ptsCaptureRed += nbRedTankIn;

        //make sure we dont go above points for cap
        if (ptsCaptureRed > maxPtsForCap)
        {
            ptsCaptureRed = maxPtsForCap;
        }
    }

    public void DecreasePtsCapBlue()
    {
        ptsCaptureBlue -= nbRedTankIn + 1; //decrease by number of enemy tanks + 1
        //make sure we dont go below 0
        if (ptsCaptureBlue < 0)
        {
            ptsCaptureBlue = 0;
        }
    }

    public void DecreasePtsCapRed()
    {
        ptsCaptureRed -= nbBlueTankIn + 1; //decrease by number of enemy tanks + 1
        //make sure we dont go below 0
        if (ptsCaptureRed < 0)
        {
            ptsCaptureRed = 0;
        }
    }

    public void CooldownTick()
    {
        currCooldown += Time.deltaTime;
        if (currCooldown >= ptsCooldown)
        {
            isCooldown = false;
            currCooldown = 0f;
        }
    }

    public void LongCooldownTick()
    {
        currCooldown += Time.deltaTime;
        if (currCooldown >= ptsLongCooldown)
        {
            isCooldown = false;
            currCooldown = 0f;
        }
    }

}
