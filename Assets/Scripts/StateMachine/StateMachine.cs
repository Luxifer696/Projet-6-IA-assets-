using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState currentState;
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

        // GET A NAME TO DISPLAY DEPENDING ON CURRENT STATE //
        if (currentState.name == "ZoneBaseState")
        {
            zoneCapLabel = "Zone is not captured!";
        }

        if (currentState.name == "ZoneCapturedState")
        {
            // FIX THIS IT DOESNT WORK
            if (currentState.ptsCaptureBlue > currentState.ptsCaptureRed)
            {
                zoneCapLabel = "Zone controlled by Blue";
            }
            else
            {
                zoneCapLabel = "Zone controlled by Red";
            }
        }

        if (currentState.name == "ZoneContestedState")
        {
            zoneCapLabel = "Zone is contested!";
        }
    }

    public void ChangeState(BaseState newState, int ptsCaptureBlue, int ptsCaptureRed, int nbTankBlueIn, int nbTankRedIn)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter(ptsCaptureBlue, ptsCaptureRed, nbTankBlueIn, nbTankRedIn);
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    private void OnGUI()
    {
        GUILayout.Label($"<color='black'><size=40>{zoneCapLabel}</size></color>");
        GUILayout.Label($"<color='blue'><size=40>{currentState.ptsCaptureBlue}</size></color>");
        GUILayout.Label($"<color='red'><size=40>{currentState.ptsCaptureRed}</size></color>");
    }
}
