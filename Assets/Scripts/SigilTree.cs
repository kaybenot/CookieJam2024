using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SigilTreeNode
{
    public List<SigilTreeNode> Nodes;
    public Sigil Sigil;
    public int BuyHpCost = 1;
    public bool Unlocked = false;
}

[Serializable]
public class SigilTree
{
    public List<SigilTreeNode> Roots;
}
