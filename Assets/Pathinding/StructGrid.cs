
using System.Collections.Generic;
using UnityEngine;

public struct StructGrid
{
    public Vector3 OriginPoint;
    public Vector3 TopRightForwardPoint;
    public Vector3 BottomLeftBackwardPoint;

    public List<Vector3> PointList;

    public StructGrid (Vector3 newOriginPoint, float MaxHorizontalDistance, float MaxVerticalDistance)
    {
        OriginPoint = newOriginPoint;

        TopRightForwardPoint = new Vector3(
            OriginPoint.x + MaxHorizontalDistance / 2,
            OriginPoint.y + MaxVerticalDistance / 2,
            OriginPoint.z + MaxHorizontalDistance / 2);

        BottomLeftBackwardPoint = new Vector3(
            OriginPoint.x - MaxHorizontalDistance / 2,
            OriginPoint.y - MaxVerticalDistance / 2,
            OriginPoint.z - MaxHorizontalDistance / 2);

        PointList = new List<Vector3>();

        FillPointList();
    }

    public void FillPointList()
    {
        if (OriginPoint != null && BottomLeftBackwardPoint != null && TopRightForwardPoint != null)
        {
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

    public bool Contains(Vector3 position)
    {
        return PointList.Contains(position);
    }

    // Ces deux méthodes créent une hitbox pour détecter si un objet est présent dans l'une des position du quadrillage
    public bool IsPointOverlapping(Vector3 areaCenter)
    {
        if (Contains(areaCenter))
        {
            //Use the OverlapBox to detect if there are any other colliders within this box area.
            //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
            Vector3 areaSize = new Vector3(0.5f, 0.5f, 0.5f);
            Collider[] hitColliders = Physics.OverlapBox(areaCenter, areaSize, Quaternion.identity);

            // Go inside the loop if object detected
            for (int indice = 0; indice < hitColliders.Length; indice++)
            {
                //Debug.Log("Hit : " + hitColliders[indice].name);

                return true;
            }
        }
        else
        {
            Debug.LogError("positionToLookAt is not in the index !");
        }

        // No object detected
        return false;
    }

    public GameObject GetOverlappingObject(Vector3 areaCenter)
    {
        if (Contains(areaCenter))
        {
            //Use the OverlapBox to detect if there are any other colliders within this box area.
            //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
            Vector3 areaSize = new Vector3(0.5f, 0.5f, 0.5f);
            Collider[] hitColliders = Physics.OverlapBox(areaCenter, areaSize, Quaternion.identity);

            // Go inside the loop if object detected
            for (int indice = 0; indice < hitColliders.Length; indice++)
            {
                //Debug.Log("Hit : " + hitColliders[indice].name);

                return hitColliders[indice].gameObject;
            }
        }
        else
        {
            Debug.LogError("positionToLookAt is not in the index !");
        }

        // No object detected
        return null;
    }
}
