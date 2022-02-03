using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnnemyRunner : MonoBehaviour
{

    BehaviourTree tree;

    // Start is called before the first frame update
    void Start()
    {
        // A lire de bas en haut pour l'odre des Nodes 

        //Instanciation du BehaviourTree
        tree = ScriptableObject.CreateInstance<BehaviourTree>();



        //Script utilsé plusieurs fois dans l'arbe donc instancié au début 
        //On cherche l'ennemi le plus proche, on tire et on attend 1 seconde tant qu'on a un ennemi dans la range de tirs
        var FaceClosestEnemy = ScriptableObject.CreateInstance<FaceClosestEnemy>();
        var shoot = ScriptableObject.CreateInstance<ShootNode>();
        var WaitEnemy = ScriptableObject.CreateInstance<WaitNode>();
        WaitEnemy.duration = 1f;

        var AttackEnemy = ScriptableObject.CreateInstance<SequencerNode>();
        AttackEnemy.children.Add(FaceClosestEnemy);
        AttackEnemy.children.Add(shoot);
        AttackEnemy.children.Add(WaitEnemy);

        //On repeat le pattern d'attaque tant qu'on a un enemy dans la range
        var RepeatAttack = ScriptableObject.CreateInstance<RepeatNode>();
        RepeatAttack.child = AttackEnemy;

        var EnemyInRange = ScriptableObject.CreateInstance<EnemyInRangeNode>();
        EnemyInRange.child = RepeatAttack;


        //Sequance de patrouille dans la zone, on va à un point randome dans la zone et on attend 1 seconde
        var MoveToRandomZonePoint = ScriptableObject.CreateInstance<MoveToRandomZonePoint>();
        var WaitInZone = ScriptableObject.CreateInstance<WaitNode>();
        WaitInZone.duration = 1f;

        var ZoneControl = ScriptableObject.CreateInstance<InvertNode>();
        ZoneControl.child = MoveToRandomZonePoint;
        ZoneControl.child = WaitInZone;





        var GetZoneControlSelector = ScriptableObject.CreateInstance<SelectorNode>();
        GetZoneControlSelector.children.Add(EnemyInRange);
        GetZoneControlSelector.children.Add(ZoneControl);



        var ZoneCloseAndNotCaptured = ScriptableObject.CreateInstance<ZoneClosAndNotCaptured>();
        ZoneCloseAndNotCaptured.child = GetZoneControlSelector;

        //On va à un point aléatoir dans la map
        var MoveToRandomPoint = ScriptableObject.CreateInstance<MoveToRandomPoint>();

        //Si il n'y a pas d'enemy en range on patrouille la map, si on est proche de la zone on la capture sinon on va à un point random
        var PatrolMap = ScriptableObject.CreateInstance<SelectorNode>();
        PatrolMap.children.Add(ZoneCloseAndNotCaptured);
        PatrolMap.children.Add(MoveToRandomPoint);




        //Node Selector qui va décider entre aller dans la zone ou si on y est
        var AIState = ScriptableObject.CreateInstance<SelectorNode>();
        AIState.children.Add(EnemyInRange);
        //AIState.children.Add();

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
