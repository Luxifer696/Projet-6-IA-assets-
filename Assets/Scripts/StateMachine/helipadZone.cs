using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class helipadZone : MonoBehaviour
{
    public int blueTeamPoints = 0;
    //private List<TankMovement> _tankList;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "red")
        {
            Debug.Log("red");
        }
        if (other.tag == "blue")
        {
            Debug.Log("blue");
        }
        else
        {
            Debug.Log("log");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)){
             
        }
    }
}
