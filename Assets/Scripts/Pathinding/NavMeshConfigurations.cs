using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshConfigurations: MonoBehaviour
{
    public ObstacleAvoidanceType AvoidanceType;

    public float AvoidancePredictionTime = 2;
    public int PathfindingIterationsPerFrame = 100;

    private void Update()
    {
        NavMesh.avoidancePredictionTime = AvoidancePredictionTime;
        NavMesh.pathfindingIterationsPerFrame = PathfindingIterationsPerFrame;
    }
}
