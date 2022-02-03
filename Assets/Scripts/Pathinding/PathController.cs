using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;
using System;

namespace Complete
{
    public class PathController : MonoBehaviour
    {
        public Camera MainCamera;
        float m_CameraSpeed = 12f;

        public List<GameObject> TankList = new List<GameObject>();

        public NavigationAI MyNavigation;

        public bool UseNaveMesh = false;

        private Thread thread;

        void Update()
        {
            #region cameraController
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
            #endregion

            if (Input.GetMouseButtonDown(0))
            {
                Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {
                    // Avec le système GRID
                    if (!UseNaveMesh)
                    {
                        Vector3 tankDestination = MyNavigation.GetClosestGridPoint(hit.point);

                        foreach (var tank in TankList)
                        {
                            Vector3 tankPositionRelativeToGrid = MyNavigation.GetClosestGridPoint(tank.transform.position);

                            // UnityException: get_defaultPhysicsScene can only be called from the main thread.
                            // Impossible donc d'utilise un thread pour construire mon path
                            /*Action act = () => MyGrid.CreatePath(tankPositionRelativeToGrid, tankDestination);
                            thread = new Thread(new ThreadStart(act));
                            thread.Start();

                            StartCoroutine(WaitThread(tank));*/

                            MyNavigation.CreatePath(tankPositionRelativeToGrid, tankDestination);
                            List<Vector3> itinary = MyNavigation.GetPath();

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

        private IEnumerator WaitThread(GameObject tank)
        {
            while (thread.IsAlive)
            {
                yield return null;
            }

            List<Vector3> itinary = MyNavigation.GetPath();

            StartCoroutine(tank.GetComponent<TankMovement>().SetItinary(itinary));
        }
    }
}
