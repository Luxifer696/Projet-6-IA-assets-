using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathinding : ScriptableObject
{
    // Mon ensemble de coordonnées
    protected StructGrid _structGrid;

    protected void SetStructGrid(StructGrid newStructGrid)
    {
        _structGrid = newStructGrid;
    }

    public abstract void CreatePath(StructGrid newStructGrid, Vector3 origin, Vector3 destination);

    public abstract List<Node> GetNodeList();

    public abstract List<Vector3> GetDirectPath();
}
