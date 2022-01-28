using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pathinding/Dijskra")]
public class Dijkstra : Pathinding
{
    // Liste de position avec leur direction de provenance
    private Dictionary<Vector3, Vector3> _pathDictionary = new Dictionary<Vector3, Vector3>();

    private List<Vector3> _positionsToCheck = new List<Vector3>();

    // Chemin reliant deux positions
    private List<Vector3> _directPath = new List<Vector3>();

    // Conditions d'arrêt
    private bool _isTherePositionToCheck;
    private bool _didIReachTheDestination = false;

    private int _security = 0;

    private void ResetVariables()
    {
        _pathDictionary.Clear();
        _positionsToCheck.Clear();
        _directPath.Clear();

        _didIReachTheDestination = false;
        _security = 0;
    }

    public override void CreatePath(StructGrid newStructGrid, Vector3 origin, Vector3 destination)
    {
        SetStructGrid(newStructGrid);
        ResetVariables();

        // Si mon origine et ma destination sont compris dans mon quadrillage
        if (_structGrid.Contains(origin) && _structGrid.Contains(destination))
        {
            _pathDictionary.Add(origin, Vector3.zero); // Vector3.zero car il proviens de nul part
            _positionsToCheck.Add(origin);

            // Tant que j'ai des positions à check
            do
            {
                LookAllSides(_positionsToCheck[0], destination);
                _positionsToCheck.RemoveAt(0);

                _security++;
            } while (_isTherePositionToCheck || _security < 500 && !_didIReachTheDestination);

            // Si je n'ai plus de position à check sans avoir atteins ma destination
            if (!_isTherePositionToCheck && !_didIReachTheDestination)
            {
                Debug.Log("Path impossible !");
            }
        }
        else
        {
            Debug.LogError("origin or destination are not in the index !");
        }
    }

    private void LookAllSides(Vector3 position, Vector3 destination)
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
            if (KeyValue.Key == destination)
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

    public override List<Vector3> GetPositionsToCheck()
    {
        return _positionsToCheck;
    }

    public override List<Node> GetNodeList()
    {
        return new List<Node>(); // a changer
    }

    public override List<Vector3> GetDirectPath()
    {
        return _directPath;
    }
}
