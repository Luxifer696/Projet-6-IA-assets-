
using System.Collections.Generic;
using UnityEngine;

public struct StructGrid
{
    public Vector3 OriginPoint;
    public Vector3 TopRightForwardPoint;
    public Vector3 BottomLeftBackwardPoint;

    public List<Vector3> PointList;

    public void FillPointList()
    {
        if (OriginPoint != null && BottomLeftBackwardPoint != null && TopRightForwardPoint != null)
        {
            PointList = new List<Vector3>(); // Instanciement de la list (sinon nullRef)
            Vector3 currentPoint = BottomLeftBackwardPoint; // Le remplissage se fait depuis le point BottomLeftBackwardPoint

            while (currentPoint.x <= TopRightForwardPoint.x)
            {
                while (currentPoint.y <= TopRightForwardPoint.y)
                {
                    while (currentPoint.z <= TopRightForwardPoint.z)
                    {
                        PointList.Add(currentPoint);

                        currentPoint = new Vector3(currentPoint.x, currentPoint.y, currentPoint.z + 1);
                    }
                    currentPoint = new Vector3(currentPoint.x, currentPoint.y + 1, BottomLeftBackwardPoint.z);
                }
                currentPoint = new Vector3(currentPoint.x + 1, BottomLeftBackwardPoint.y, currentPoint.z);
            }
        }
        else
        {
            Debug.LogError("You can't fill the point list !");
        }
    }

    public void ShowPointList()
    {
        foreach(var point in PointList)
        {
            Debug.Log(point + "\t");
        }
    }

    public bool GetOverlapPoint(int pointIndice)
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Vector3 positionToLookAt = PointList[pointIndice];
        Vector3 detectionAreaSize = new Vector3(0.5f, 0.5f, 0.5f);
        Collider[] hitColliders = Physics.OverlapBox(positionToLookAt, detectionAreaSize, Quaternion.identity);

        Debug.Log("CC");

        //Check when there is a new collider coming into contact with the box
        for (int indice = 0; indice < hitColliders.Length; indice++)
        {
            Debug.Log("Hit : " + hitColliders[indice].name);

            if(hitColliders[indice] != null)
            {
                // Object detected
                return true;
            }
        }

        // No object detected
        return false;
    }
}
