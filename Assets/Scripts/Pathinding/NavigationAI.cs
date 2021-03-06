using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationAI : MonoBehaviour
{
    // Update la position de la grille si l'on d?cide de la bouger en jeu
    [SerializeField]
    private bool UpdateGridPosition = false;

    // Centre du quadrillage
    [Tooltip("GameObject position by default")]
    public Vector3 OriginPoint;

    // Param?tre du quadrillage (nbr pair)
    [Tooltip("Should be an even number")]
    public int MaxHorizontalDistance = 40;
    [Tooltip("Should be an even number")]
    public int MaxVerticalDistance = 4;

    // Guizmos
    [SerializeField]
    private bool ShowGrid = false;

    [SerializeField]
    private bool ShowPathindingSearch = true;

    [SerializeField]
    private bool ShowPathindingDirectPath = true;

    // Enregistre toute les coordonn?es
    private StructGrid _structGrid;

    // Pathinding
    [SerializeField]
    private Pathinding _pathinding;

    private void Awake()
    {
        OriginPoint = transform.position;
        _structGrid = new StructGrid(OriginPoint, MaxHorizontalDistance, MaxVerticalDistance);
    }

    void Start()
    {
        //_pathinding.CreatePath(_structGrid, OriginPoint + Vector3.up / 2, OriginPoint + Vector3.up / 2 + Vector3.right * 3 + Vector3.back * 7);
        //_structGrid.ShowPointList();

        //Debug.Log(_structGrid.IsPointOverlapping(new Vector3(-11, 1, -9)));

        //Debug.Log(_structGrid.BottomLeftBackwardPoint);
        //_structGrid.ShowPointList();
        //_structGrid.IsPointOverlapping(_structGrid.PointList[0]);
    }

    private void Update()
    {
        if(UpdateGridPosition)
        {
            OriginPoint = transform.position;
            _structGrid = new StructGrid(OriginPoint, MaxHorizontalDistance, MaxVerticalDistance);
            //_pathinding.CreatePath(_structGrid, OriginPoint + Vector3.up / 2, OriginPoint + Vector3.up / 2 + Vector3.right * 3 + Vector3.back * 7);

            UpdateGridPosition = false;
        }
    }

    public void CreatePath(Vector3 origin, Vector3 destination)
    {
        _pathinding.CreatePath(_structGrid, origin, destination);
    }

    public List<Vector3> GetPath()
    {
        return _pathinding.GetDirectPath();
    }

    private void ThreadFunc()
    {

    }

    public Vector3 GetClosestGridPoint(Vector3 position)
    {
        Vector3 closestPoint = _structGrid.PointList[0];

        foreach (Vector3 point in _structGrid.PointList)
        {
            if (Vector3.Distance(position, point) < Vector3.Distance(position, closestPoint))
            {
                if (_structGrid.IsThisPositionFree(point))
                {
                    closestPoint = point;
                }
            }
        }

        return closestPoint;
    }

    public StructGrid GetStructGrid()
    {
        return _structGrid;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_structGrid.OriginPoint, 0.2f);
        Gizmos.DrawSphere(_structGrid.TopRightForwardPoint, 0.2f);
        Gizmos.DrawSphere(_structGrid.BottomLeftBackwardPoint, 0.2f);

        if (ShowGrid)
        {
            Gizmos.color = Color.blue;
            foreach (var point in _structGrid.PointList)
            {
                Gizmos.DrawSphere(point, 0.05f);
            }
        }

        if (ShowPathindingSearch)
        {
            Gizmos.color = Color.red;
            foreach (var node in _pathinding.GetNodeList())
            {
                Gizmos.DrawWireCube(node.Position, new Vector3(1, 1, 1));
            }
        }

        if (ShowPathindingDirectPath)
        {
            Gizmos.color = Color.green;
            foreach (var point in _pathinding.GetDirectPath())
            {
                Gizmos.DrawWireCube(point, new Vector3(1, 1, 1));
            }
        }
    }
}
