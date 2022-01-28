using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Node
{
    public Vector3 Position;
    public Vector3 PreviousNodePosition;

    public float GCost; // Distance with starting node
    public float HCost; // Distance with end node
    public float FCost;

    public bool HasBeenChecked;

    public Node(Vector3 newPosition, Vector3 newPreviousNodePosition, float newGCost = 0, float newHCost = 0)
    {
        Position = newPosition;
        PreviousNodePosition = newPreviousNodePosition;

        GCost = newGCost;
        HCost = newHCost;
        FCost = GCost + HCost;

        HasBeenChecked = false;
    }

    public Vector3 GetPreviousNodePosition()
    {
        return PreviousNodePosition;
    }
}
