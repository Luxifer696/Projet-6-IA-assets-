using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string name;
    protected StateMachine stateMachine;

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

    public virtual void Exit() { }

    public virtual void UpdateLogic() { }

    public virtual void UpdatePhysics() { }
    
}
