using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Complete
{
    public class GodController : MonoBehaviour
    {
        public Camera MainCamera;
        float m_CameraSpeed = 12f;

        public List<GameObject> TankList = new List<GameObject>();

        public Grid MyGrid;

        public bool UseNaveMesh = false;

        void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MainCamera.transform.position += MainCamera.transform.up * m_CameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                MainCamera.transform.position += -MainCamera.transform.up * m_CameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                MainCamera.transform.position += MainCamera.transform.right * m_CameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MainCamera.transform.position += -MainCamera.transform.right * m_CameraSpeed * Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {
                    // Avec le système GRID
                    if (!UseNaveMesh)
                    {
                        Vector3 tankDestination = MyGrid.GetClosestGridPoint(hit.point);

                        foreach (var tank in TankList)
                        {
                            Vector3 tankPositionRelativeToGrid = MyGrid.GetClosestGridPoint(tank.transform.position);

                            List<Vector3> itinary = MyGrid.GetPath(tankPositionRelativeToGrid, tankDestination);

                            StartCoroutine(tank.GetComponent<TankMovement>().SetItinary(itinary));
                        }
                    }
                    // Avec le système NavMesh
                    else
                    {
                        foreach (var tank in TankList)
                        {
                            NavMeshAgent agent = tank.GetComponent<NavMeshAgent>();

                            if(agent)
                            {
                                agent.SetDestination(hit.point);
                            }
                        }
                    }
                }
            }
        }
    }
}
