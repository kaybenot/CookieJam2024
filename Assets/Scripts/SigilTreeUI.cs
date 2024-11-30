using System;
using System.Linq;
using UnityEngine;

public class SigilTreeUI : MonoBehaviour
{
    [SerializeField] private SigilDescriptionUI descriptionUI;
    [SerializeField] private Transform tier1Transform;
    [SerializeField] private Transform tier2Transform;
    [SerializeField] private Transform tier3Transform;
    [SerializeField] private GameObject SigilNodePrefab;
    
    public static SigilTreeUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    [ContextMenu("Show Nodes")]
    public void Show()
    {
        gameObject.SetActive(true);

        var tree = FindAnyObjectByType<Player>().SigilTree;

        foreach (var node in tree.Roots)
        {
            var sigilUI = Instantiate(SigilNodePrefab, tier1Transform).GetComponent<SigilNodeUI>();
            sigilUI.Node = node;
            sigilUI.DescriptionUI = descriptionUI;
        }

        var t2 = tree.Roots.SelectMany(n => n.Nodes);
        foreach (var node in t2)
        {
            var sigilUI = Instantiate(SigilNodePrefab, tier2Transform).GetComponent<SigilNodeUI>();
            sigilUI.Node = node;
            sigilUI.DescriptionUI = descriptionUI;
        }
        
        var t3 = tree.Roots.SelectMany(n => n.Nodes.SelectMany(m => m.Nodes));
        foreach (var node in t3)
        {
            var sigilUI = Instantiate(SigilNodePrefab, tier3Transform).GetComponent<SigilNodeUI>();
            sigilUI.Node = node;
            sigilUI.DescriptionUI = descriptionUI;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
