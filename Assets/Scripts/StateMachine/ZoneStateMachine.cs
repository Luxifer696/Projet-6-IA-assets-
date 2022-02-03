using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneStateMachine : StateMachine
{
    [HideInInspector] 
    public ZoneBaseState zoneBaseState;
    [HideInInspector]
    public ZoneCapturedState zoneCapturedState;

    [HideInInspector]
    public ZoneContestedState zoneContestedState;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "red")
        {
            currentState.nbRedTankIn++;
            if (currentState.nbBlueTankIn == 0)
            {
                Debug.Log("red capturing!");
            }
        }
        if (other.tag == "blue")
        {
            currentState.nbBlueTankIn++;
            if (currentState.nbRedTankIn == 0)
            {
                Debug.Log("blue capturing!");
            }
        }
        else
        {
            
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "red")
        {
            currentState.nbRedTankIn--;
        }
        if (other.tag == "blue")
        {
            currentState.nbBlueTankIn--;
        }
        else
        {

        }
    }


    private void Awake()
    {
        zoneBaseState = new ZoneBaseState(this);
        zoneCapturedState = new ZoneCapturedState(this);
        zoneContestedState = new ZoneContestedState(this);
    }

    //this is the part that gives zoneBaseState as first state !!
    protected override BaseState GetInitialState()
    {
        return zoneBaseState;
    }
}
