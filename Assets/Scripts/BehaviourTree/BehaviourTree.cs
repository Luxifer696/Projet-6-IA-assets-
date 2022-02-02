using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public NodeBT rootNode;
    public NodeBT.State treeState = NodeBT.State.RUNNING;
    public List<NodeBT> nodes = new List<NodeBT>(); 

    public NodeBT.State Update(){
        if (rootNode.state == NodeBT.State.RUNNING)
        {
            treeState = rootNode.Update(); 
        }
        return treeState; 
    }

    public NodeBT CreateNode(System.Type type)
    {
        NodeBT node = ScriptableObject.CreateInstance(type) as NodeBT;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode( NodeBT node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets(); 
    }
}
