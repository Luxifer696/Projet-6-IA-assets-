using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pathinding/A*")]
public class Astar : Pathinding
{
    // Liste de node contenant leurs positions, leurs positions precedentes et leurs distances avec l'origin et la destionation
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

            LookAllSides(origin, origin, destination);
            CheckNode(originNode);

            // Tant que j'ai des positions à check
            while (_isTherePositionToCheck || _securityInfinityLoop < 500 && !_didIReachTheDestination)
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

    private void LookAllSides(Vector3 position, Vector3 origin, Vector3 destination)
    {
        _isTherePositionToCheck = false;

        // Dictionnaire avec les nouvelles positions et leur provenances
        List<Node> nearNodesList = new List<Node>();

        Vector3 newPosition = default;

        // Extraction de nouvelles nodes
        for(int nbrSide = 26; nbrSide > 0; nbrSide--)
        {
            // Prend en compte les positions adjacantes et les diagonales (forme un cube de recherche au final)
            switch(nbrSide)
            {
                case 1: newPosition = position + Vector3.forward; break;
                case 2: newPosition = position + Vector3.back; break;
                case 3: newPosition = position + Vector3.right; break;
                case 4: newPosition = position + Vector3.left; break;
                case 5: newPosition = position + Vector3.up; break;
                case 6: newPosition = position + Vector3.down; break;
                case 7: newPosition = position + Vector3.forward + Vector3.right; break;
                case 8: newPosition = position + Vector3.forward + Vector3.left; break;
                case 9: newPosition = position + Vector3.forward + Vector3.up; break;
                case 10: newPosition = position + Vector3.forward + Vector3.down; break;
                case 11: newPosition = position + Vector3.back + Vector3.right; break;
                case 12: newPosition = position + Vector3.back + Vector3.left; break;
                case 13: newPosition = position + Vector3.back + Vector3.up; break;
                case 14: newPosition = position + Vector3.back + Vector3.down; break;
                case 15: newPosition = position + Vector3.right + Vector3.up; break;
                case 16: newPosition = position + Vector3.right + Vector3.down; break;
                case 17: newPosition = position + Vector3.left + Vector3.up; break;
                case 18: newPosition = position + Vector3.left + Vector3.down; break;
                case 19: newPosition = position + Vector3.forward + Vector3.left + Vector3.up; break;
                case 20: newPosition = position + Vector3.forward + Vector3.left + Vector3.down; break;
                case 21: newPosition = position + Vector3.forward + Vector3.right + Vector3.up; break;
                case 22: newPosition = position + Vector3.forward + Vector3.right + Vector3.down; break;
                case 23: newPosition = position + Vector3.back + Vector3.left + Vector3.up; break;
                case 24: newPosition = position + Vector3.back + Vector3.left + Vector3.down; break;
                case 25: newPosition = position + Vector3.back + Vector3.right + Vector3.up; break;
                case 26: newPosition = position + Vector3.back + Vector3.right + Vector3.down; break;
            }

            nearNodesList.Add(new Node(newPosition, position, Vector3.Distance(newPosition, origin), Vector3.Distance(newPosition, destination)));
        }

        // Traitement des nouvelles nodes
        foreach (Node node in nearNodesList)
        {
            // Si j'atteins ma destination
            if (node.Position == destination)
            {
                _nodesList.Add(node);
                CreateReversePath(node);

                _didIReachTheDestination = true;
                return;
            }

            // Si je suis toujours dans mon quadrillage ET si je n'ai pas déjà regarder cette position
            if (_structGrid.Contains(node.Position) && !_didIReachTheDestination)
            {
                if (!HasThisNodePosition(node.Position))
                {
                    // S'il n'y a pas d'objet à cette position OU s'il s'agit du joueur (de son propre corps)
                    if (!_structGrid.IsPointOverlapping(node.Position) || _structGrid.GetOverlappingObject(node.Position).layer == 9)
                    {
                        _nodesList.Add(node);

                        _isTherePositionToCheck = true;
                    }
                }
                else
                {
                    // Si j'ai deux nodes avec une même position, je garde celle qui a un FCost plus petit
                    if(GetNodeWithPosition(node.Position).FCost > node.FCost)
                    {
                        _nodesList.Remove(GetNodeWithPosition(node.Position));
                        _nodesList.Add(node);
                    }
                }
            }
        }
    }

    private bool HasThisNodePosition(Vector3 position)
    {
        foreach(Node node in _nodesList)
        {
            if(node.Position == position)
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
        Node nodeWithLowerHCost = default;

        // On prend la première Node non chech
        foreach (Node node in _nodesList)
        {
            if (!node.HasBeenChecked)
            {
                nodeWithLowerHCost = node;
                break;
            }
        }

        // On la compare aux autres pour garder celle avec le plus bas FCost
        foreach (Node node in _nodesList)
        {
            if (!node.HasBeenChecked)
            {
                if (nodeWithLowerHCost.FCost > node.FCost)
                {
                    nodeWithLowerHCost = node;
                }
                else if (nodeWithLowerHCost.FCost == node.FCost) // En cas d'égalité on prend celui le plus proche de la destination (lower HCost)
                {
                    if (nodeWithLowerHCost.HCost > node.HCost)
                    {
                        nodeWithLowerHCost = node;
                    }
                }
            }
        }
        return nodeWithLowerHCost;
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
    }

    public override List<Vector3> GetPositionsToCheck()
    {
        return default; // a changer
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
