using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    private int nbCapBlue;
    
    void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
        {
            currentState.Enter();
        }
    }
    
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateLogic();
        }
    }
    
    void LateUpdate()
    {
        if (currentState != null)
        {
            currentState.UpdatePhysics();
        }

        nbCapBlue = currentState.GetNbPointBlue();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    private void OnGUI()
    {
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.Label($"<color='blue'><size=40>{nbCapBlue}</size></color>");
    }
}
