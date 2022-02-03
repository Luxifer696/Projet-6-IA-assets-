using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneControlRunner : MonoBehaviour
{

    BehaviourTree tree;

    // Start is called before the first frame update
    void Start()
    {

        // A lire de bas en haut pour l'odre des Nodes 

        //Instanciation du BehaviourTree
        tree = ScriptableObject.CreateInstance<BehaviourTree>();

        //Sequance ou le tank se déplace vers la zone
        var GetZoneLocation = ScriptableObject.CreateInstance<GetZoneLocation>();
        var MoveToPoint = ScriptableObject.CreateInstance<MoveToPoint>();

        var GoToZone = ScriptableObject.CreateInstance<SequencerNode>();
        GoToZone.children.Add(GetZoneLocation);
        GoToZone.children.Add(MoveToPoint); 

        //Sequance de patrouille dans la zone, on va à un point randome dans la zone et on attend 1 seconde
        var MoveToRandomPoint = ScriptableObject.CreateInstance<MoveToRandomZonePoint>();
        var WaitInZone = ScriptableObject.CreateInstance<WaitNode>();
        WaitInZone.duration = 1f;

        var PatrolZone = ScriptableObject.CreateInstance<SequencerNode>();
        PatrolZone.children.Add(MoveToRandomPoint);
        PatrolZone.children.Add(WaitInZone); 

        //On cherche l'ennemi le plus proche, on tire et on attend 1 seconde tant qu'on a un ennemi dans la range de tirs
        var FaceClosestEnemy = ScriptableObject.CreateInstance<FaceClosestEnemy>();
        var shoot = ScriptableObject.CreateInstance<ShootNode>();
        var WaitEnemy = ScriptableObject.CreateInstance<WaitNode>();
        WaitEnemy.duration = 1f; 

        var AttackEnemy = ScriptableObject.CreateInstance<SequencerNode>();
        AttackEnemy.children.Add(FaceClosestEnemy);
        AttackEnemy.children.Add(shoot);
        AttackEnemy.children.Add(WaitEnemy); 

        var EnemyInRange = ScriptableObject.CreateInstance<EnemyInRangeNode>();
        EnemyInRange.child = AttackEnemy; 

        //Si On a un ennmi dans la range de tir on active la sequence de tir, sinon on patrouille autour de la zone tout en restant de dedans
        var ControlZone = ScriptableObject.CreateInstance<SelectorNode>();
        ControlZone.children.Add(EnemyInRange);
        ControlZone.children.Add(PatrolZone);

        
        //Décorator qui nous dit si on est dans la zone ou non 
        var IsInZone = ScriptableObject.CreateInstance<isInZoneNode>();
        IsInZone.child = ControlZone;

        //Node Selector qui va décider entre aller dans la zone ou si on y est
        var AIState = ScriptableObject.CreateInstance<SelectorNode>();
        AIState.children.Add(IsInZone); 
        AIState.children.Add(GoToZone);

        var RepeatTree = ScriptableObject.CreateInstance<RepeatNode>(); 
        RepeatTree.child = AIState; 
        //Node Root 
        var root = ScriptableObject.CreateInstance<RootNode>();
        root.child = RepeatTree; 
        tree.rootNode = root;
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update(); 
    }
}
