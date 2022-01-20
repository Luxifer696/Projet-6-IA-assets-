using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    // Position de recherche actuel
    private Vector3 _currentPosition;

    // Liste de position � regarder (poss�de un float repr�sentant sa priorit�)
    private Dictionary<Vector3, float> _targetPosition = new Dictionary<Vector3, float>();

    // Position � trouver
    private Vector3 _positionToFind;

    // Mon ensemble de coordonn�es
    private StructGrid _structGrid;

    void Start()
    {
        try
        {
            _structGrid = GetComponent<Grid>().GetStructGrid();
        }
        catch
        {
            Debug.LogError("GetComponent<Grid>().GetStructGrid() return null");
        }
        /*
         * foreach (KeyValuePair<GameObject, ArrayList> KeyValue in _targetDictionary)
         */
    }

    public void GetPath(Vector3 origin, Vector3 destination)
    {
        if(_structGrid.PointList.Contains(origin) && _structGrid.PointList.Contains(destination))
        {

        }
        else
        {
            Debug.LogError("origin or destination are not in the index !");
        }
    }
}
