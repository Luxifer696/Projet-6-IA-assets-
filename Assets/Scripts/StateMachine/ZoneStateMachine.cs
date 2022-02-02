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
            zoneBaseState.nbRedTankIn++;
            if (zoneBaseState.nbBlueTankIn == 0)
            {
                Debug.Log("red capturing!");
            }
        }
        if (other.tag == "blue")
        {
            zoneBaseState.nbBlueTankIn++;
            if (zoneBaseState.nbRedTankIn == 0)
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
            zoneBaseState.nbRedTankIn--;
        }
        if (other.tag == "blue")
        {
            zoneBaseState.nbBlueTankIn--;
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

    protected override BaseState GetInitialState()
    {
        return zoneBaseState;
    }
}
