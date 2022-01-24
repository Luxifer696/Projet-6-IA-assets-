using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    // Liste de position avec leur direction de provenance
    private Dictionary<Vector3, Vector3> _pathDictionary = new Dictionary<Vector3, Vector3>();

    private List<Vector3> _positionsToCheck = new List<Vector3>();

    private List<Vector3> _directPath = new List<Vector3>();

    private Vector3 _destination;

    // Mon ensemble de coordonnées
    private StructGrid _structGrid;

    private bool _isTherePositionToCheck;
    private bool _didIReachTheDestination = false;

    private int _security = 0;

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
    }

    public void CreatePath(Vector3 origin, Vector3 destination)
    {
        // Si mon origine et ma destination sont compris dans mon quadrillage
        if (_structGrid.Contains(origin) && _structGrid.Contains(destination))
        {
            _pathDictionary.Add(origin, Vector3.zero); // Vector3.zero car il ne proviens de nul part
            _positionsToCheck.Add(origin); 
            _destination = destination;

            while (_isTherePositionToCheck || _security < 500 && !_didIReachTheDestination)
            {
                LookAllSides(_positionsToCheck[0]);
                _positionsToCheck.RemoveAt(0);

                _security++;
            }
        }
        else
        {
            Debug.LogError("origin or destination are not in the index !");
        }
    }

    private void LookAllSides(Vector3 position)
    {
        _isTherePositionToCheck = false;

        // Dictionnaire avec les nouvelles positions et leur provenances
        Dictionary<Vector3, Vector3> sidePositionsAndOrigin = new Dictionary<Vector3, Vector3>();

        sidePositionsAndOrigin.Add( position + Vector3.forward, Vector3.back );
        sidePositionsAndOrigin.Add( position + Vector3.right, Vector3.left );
        sidePositionsAndOrigin.Add( position + Vector3.back, Vector3.forward );
        sidePositionsAndOrigin.Add( position + Vector3.left, Vector3.right );
        sidePositionsAndOrigin.Add( position + Vector3.up, Vector3.down );
        sidePositionsAndOrigin.Add( position + Vector3.down, Vector3.up );

        // Pour chaque positions annexes
        foreach (KeyValuePair<Vector3, Vector3> KeyValue in sidePositionsAndOrigin)
        {
            // Si j'atteins ma destination
            if(KeyValue.Key == _destination)
            {
                _pathDictionary.Add(KeyValue.Key, KeyValue.Value);
                CreateReversePath(KeyValue.Key);

                _didIReachTheDestination = true;
                return;
            }

            // Si je suis toujours dans mon quadrillage ET si je n'ai pas déjà regarder cette position
            if (_structGrid.Contains(KeyValue.Key) && !_pathDictionary.ContainsKey(KeyValue.Key) && !_didIReachTheDestination)
            {
                // S'il n'y a pas d'object à cette position OU s'il s'agit du joueur (de son propre corps)
                if (!_structGrid.IsPointOverlapping(KeyValue.Key) || _structGrid.GetOverlappingObject(KeyValue.Key).layer == 9)
                {
                    _pathDictionary.Add(KeyValue.Key, KeyValue.Value);
                    _positionsToCheck.Add(KeyValue.Key);

                    _isTherePositionToCheck = true;
                }
            }
        }
    }

    private void CreateReversePath(Vector3 destination)
    {
        Vector3 path = destination;
        _directPath.Add(path);

        // Tant que mon chemin proviens de quelque part
        while (path + _pathDictionary[path] != path)
        {
            path += _pathDictionary[path];
            _directPath.Add(path);
        }
    }

    public List<Vector3> GetPositionsToCheck()
    {
        return _positionsToCheck;
    }

    public Dictionary<Vector3, Vector3> GetWayDictionary()
    {
        return _pathDictionary;
    }

    public List<Vector3> GetDirectPath()
    {
        return _directPath;
    }
}
