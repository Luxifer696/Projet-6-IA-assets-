using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pathinding/Dijskra")]
public class Dijkstra : Pathinding
{
    // Liste de node contenant leurs positions et leurs positions precedentes
    private List<Node> _nodesList = new List<Node>();

    // Chemin reliant deux positions
    private List<Vector3> _directPath = new List<Vector3>();

    // Conditions d'arrêt
    private bool _isTherePositionToCheck;
    private bool _didIReachTheDestination = false;

    private int _securityInfinityLoop = 0;

    private void ResetVariables()
    {
        _nodesList.Clear();
        _directPath.Clear();

        _didIReachTheDestination = false;
        _securityInfinityLoop = 0;
    }

    public override void CreatePath(StructGrid newStructGrid, Vector3 origin, Vector3 destination)
    {
        SetStructGrid(newStructGrid);
        ResetVariables();

        // Si mon origine et ma destination sont compris dans mon quadrillage
        if (_structGrid.Contains(origin) && _structGrid.Contains(destination))
        {
            Node originNode = new Node(origin, origin, 0, Vector3.Distance(origin, destination));
            _nodesList.Add(originNode);

            // Tant que j'ai des positions à check
            while (_isTherePositionToCheck || _securityInfinityLoop < 50000 && !_didIReachTheDestination)
            {
                Node newNodeToCheck = GetNextNodeToCheck();

                LookAllSides(newNodeToCheck.Position, origin, destination);
                CheckNode(newNodeToCheck);

                //Debug.Log(newNodeToCheck.Position);

                _securityInfinityLoop++;
            }

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

    protected override void LookAllSides(Vector3 position, Vector3 origin, Vector3 destination)
    {
        _isTherePositionToCheck = false;

        // Dictionnaire avec les nouvelles positions et leur provenances
        List<Node> nearNodesList = new List<Node>();

        Vector3 newPosition = default;

        // Extraction de nouvelles nodes
        for (int nbrSide = 26; nbrSide > 0; nbrSide--)
        {
            // Prend en compte les positions adjacantes et les diagonales (forme un cube de recherche au final)
            switch (nbrSide)
            {
                case 1: newPosition = position + Vector3.forward; break;
                case 2: newPosition = position + Vector3.back; break;
                case 3: newPosition = position + Vector3.right; break;
                case 4: newPosition = position + Vector3.left; break;
                case 5: newPosition = position + Vector3.up; break;
                case 6: newPosition = position + Vector3.down; break;
            }

            nearNodesList.Add(new Node(newPosition, position, Vector3.Distance(newPosition, origin), Vector3.Distance(newPosition, destination)));
        }

        // Traitement des nouvelles nodes
        foreach (Node node in nearNodesList)
        {
            // Si j'atteins ma destination
            if (node.Position == destination) // destination
            {
                _nodesList.Add(node);
                CreateReversePath(node);

                _didIReachTheDestination = true;
                return;
            }

            // Si je suis toujours dans mon quadrillage ET si je n'ai pas déjà regarder cette position
            if (_structGrid.Contains(node.Position) && !HasThisNodePosition(node.Position) && !_didIReachTheDestination)
            {
                // S'il n'y a pas d'object à cette position OU s'il s'agit du joueur (de son propre corps)
                if (_structGrid.IsThisPositionFree(node.Position))
                {
                    _nodesList.Add(node);

                    _isTherePositionToCheck = true;
                }
            }
        }
    }

    private bool HasThisNodePosition(Vector3 position)
    {
        foreach (Node node in _nodesList)
        {
            if (node.Position == position)
            {
                return true;
            }
        }
        return false;
    }

    private Node GetNodeWithPosition(Vector3 position)
    {
        foreach (Node node in _nodesList)
        {
            if (node.Position == position)
            {
                return node;
            }
        }
        return default;
    }

    private void CheckNode(Node nodeToCheck)
    {
        _nodesList.Remove(nodeToCheck);
        nodeToCheck.HasBeenChecked = true;
        _nodesList.Add(nodeToCheck);
    }

    private Node GetNextNodeToCheck()
    {
        Node nodeNoChecked = default;

        // On prend la première Node non check
        foreach (Node node in _nodesList)
        {
            if (!node.HasBeenChecked)
            {
                nodeNoChecked = node;
                break;
            }
        }

        return nodeNoChecked;
    }

    private void CreateReversePath(Node endNode)
    {
        _directPath.Add(endNode.Position);

        Node nextNode = GetNodeWithPosition(endNode.PreviousNodePosition);

        float securityInfinityLoop = 0;
        // Tant que mon chemin proviens de quelque part
        while (nextNode.PreviousNodePosition != nextNode.Position && securityInfinityLoop < 400)
        {
            _directPath.Add(nextNode.Position);

            nextNode = GetNodeWithPosition(nextNode.PreviousNodePosition);
            securityInfinityLoop++;
        }

        OrderDirectPath();
    }

    private void OrderDirectPath()
    {
        List<Vector3> orderedPath = new List<Vector3>();

        foreach (Vector3 position in _directPath)
        {
            orderedPath.Insert(0, position);
        }

        _directPath = orderedPath;
    }

    public override List<Node> GetNodeList()
    {
        return _nodesList;
    }

    public override List<Vector3> GetDirectPath()
    {
        return _directPath;
    }
}
