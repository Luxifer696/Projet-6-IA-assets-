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

    private void Awake()
    {
        zoneBaseState = new ZoneBaseState(this);
        zoneCapturedState = new ZoneCapturedState(this);
    }

    protected override BaseState GetInitialState()
    {
        return zoneBaseState;
    }
}
