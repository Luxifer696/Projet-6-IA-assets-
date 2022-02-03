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
    
    public virtual void Exit() { }

    public virtual void UpdateLogic() { }

    public virtual void UpdatePhysics() { }
    
}
