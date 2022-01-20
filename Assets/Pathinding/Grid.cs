using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // Update la position de la grille si l'on décide de la bouger en jeu
    [SerializeField]
    private bool UpdateGridPosition = false;

    // Centre du quadrillage
    public Vector3 OriginPoint;

    // Paramètre du quadrillage (nbr pair)
    public int MaxHorizontalDistance = 40;
    public int MaxVerticalDistance = 4;

    // Enregistre toute les coordonnées
    private StructGrid _structGrid;

    void Start()
    {
        OriginPoint = transform.position;

        SetupStructGrid();
    }

    private void Update()
    {
        if(UpdateGridPosition)
        {
            OriginPoint = transform.position;
            SetupStructGrid();
            UpdateGridPosition = false;
        }
    }

    private void SetupStructGrid()
    {
        _structGrid = new StructGrid();

        _structGrid.OriginPoint = OriginPoint;

        _structGrid.TopRightForwardPoint = new Vector3(
            OriginPoint.x + MaxHorizontalDistance / 2,
            OriginPoint.y + MaxVerticalDistance / 2,
            OriginPoint.z + MaxHorizontalDistance / 2);

        _structGrid.BottomLeftBackwardPoint = new Vector3(
            OriginPoint.x - MaxHorizontalDistance / 2,
            OriginPoint.y - MaxVerticalDistance / 2,
            OriginPoint.z - MaxHorizontalDistance / 2);

        _structGrid.FillPointList();
        //_structGrid.ShowPointList();
        _structGrid.GetOverlapPoint(0);

        //Debug.Log(_structGrid.PointList.Contains(new Vector3(0, 0.5f, 0)));
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

        Gizmos.color = Color.blue;
        foreach (var point in _structGrid.PointList)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}
