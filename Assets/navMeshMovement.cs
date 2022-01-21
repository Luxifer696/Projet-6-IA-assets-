using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class navMeshMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    public Camera cam;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        cam = Camera.main; //the only camera active now
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //move agent to camera click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                navMeshAgent.SetDestination(hit.point);
            }
        }
    }
}
