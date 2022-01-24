using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // Update la position de la grille si l'on décide de la bouger en jeu
    [SerializeField]
    private bool UpdateGridPosition = false;

    // Centre du quadrillage
    [Tooltip("GameObject position by default")]
    public Vector3 OriginPoint;

    // Paramètre du quadrillage (nbr pair)
    [Tooltip("Should be an even number")]
    public int MaxHorizontalDistance = 40;
    [Tooltip("Should be an even number")]
    public int MaxVerticalDistance = 4;

    // Guizmos
    [SerializeField]
    private bool ShowGrid = false;

    [SerializeField]
    private bool ShowDijskraSearch = false;

    [SerializeField]
    private bool ShowDijskraDirectPath = false;

    // Enregistre toute les coordonnées
    private StructGrid _structGrid;

    // Pathinding
    private Dijkstra _dijkstra;

    private void Awake()
    {
        OriginPoint = transform.position;
        _structGrid = new StructGrid(OriginPoint, MaxHorizontalDistance, MaxVerticalDistance);

        _dijkstra = GetComponent<Dijkstra>();
    }

    void Start()
    {
        _dijkstra.CreatePath(OriginPoint + Vector3.up, OriginPoint + Vector3.up + Vector3.right * 3 + Vector3.back * 7);
        //_structGrid.ShowPointList();
        //_structGrid.IsPointOverlapping(_structGrid.PointList[0]);
    }

    private void Update()
    {
        if(UpdateGridPosition)
        {
            OriginPoint = transform.position;
            _structGrid = new StructGrid(OriginPoint, MaxHorizontalDistance, MaxVerticalDistance);

            UpdateGridPosition = false;
        }
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
                Gizmos.DrawSphere(point, 0.1f);
            }
        }

        if (ShowDijskraSearch)
        {
            Gizmos.color = Color.red;
            foreach (var point in _dijkstra.GetPositionsToCheck())
            {
                Gizmos.DrawWireCube(point, new Vector3(1, 1, 1));
            }
        }

        if (ShowDijskraDirectPath)
        {
            Gizmos.color = Color.green;
            foreach (var point in _dijkstra.GetDirectPath())
            {
                Gizmos.DrawWireCube(point, new Vector3(1, 1, 1));
            }
        }
    }
}
