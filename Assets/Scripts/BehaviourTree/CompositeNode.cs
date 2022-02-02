using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : NodeBT
{
    public List<NodeBT> children = new List<NodeBT>(); 
}
