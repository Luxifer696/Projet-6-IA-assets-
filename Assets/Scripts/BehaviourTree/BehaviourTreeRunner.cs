using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{

    BehaviourTree tree; 
    // Start is called before the first frame update
    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviourTree>();

        var sequence = ScriptableObject.CreateInstance<SequencerNode>();
        //sequence.children.Add();




        var AIState = ScriptableObject.CreateInstance<SelectorNode>();
        AIState.children.Add(sequence);     
      
        var root = ScriptableObject.CreateInstance<RootNode>();
        root.child = AIState; 

        tree.rootNode = root;
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update(); 
    }
}
