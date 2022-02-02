using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeBT : ScriptableObject
{
    public enum  State
    {
        // Enumérator pour les 3 states d'une Node soit elle est en marche soit elle a finis avec Success/Failure
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public State state = State.RUNNING;
    public bool started = false;
    public string guid; 

    public State Update()
    {
        if (!started)
        {
            OnStart();
            started = true; 
        }
        state = OnUpdate(); 

        if (state == State.FAILURE || state == State.SUCCESS)
        {
            OnStop();
            started = false; 
        }

        return state; 
    }
    protected abstract void OnStart();
    protected abstract void OnStop();

    protected abstract State OnUpdate(); 
}
