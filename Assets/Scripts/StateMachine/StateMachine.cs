using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    private int nbCapBlue; 
    private int nbCapRed;
    private string zoneCapLabel;
    
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
        nbCapRed = currentState.GetNbPointRed();

        if (currentState.name == "ZoneBaseState")
        {
            zoneCapLabel = "Zone is not captured!";
        }

        if (currentState.name == "ZoneCapturedState")
        {
            if (nbCapBlue > nbCapRed)
            {
                zoneCapLabel = "Zone controlled by Blue";
            }
            else
            {
                zoneCapLabel = "Zone controlled by Red";
            }
        }
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
        GUILayout.Label($"<color='black'><size=40>{zoneCapLabel}</size></color>");
        GUILayout.Label($"<color='blue'><size=40>{nbCapBlue}</size></color>");
        GUILayout.Label($"<color='red'><size=40>{nbCapRed}</size></color>");
    }
}
