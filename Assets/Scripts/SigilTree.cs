using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SigilTreeNode
{
    [SerializeReference, FormerlySerializedAs("Nodes")]
    private List<SigilTreeNode> nodes;
    public IReadOnlyList<SigilTreeNode> Nodes => nodes;

    public Sigil Sigil;
    public int BuyHpCost = 1;
    public bool Unlocked = false;
}

[Serializable]
public class SigilTree
{
    public List<SigilTreeNode> Roots;
}
