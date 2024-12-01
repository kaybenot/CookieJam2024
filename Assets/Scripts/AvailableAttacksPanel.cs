using System.Collections.Generic;
using UnityEngine;

public class AvailableAttacksPanel : MonoBehaviour
{
    [SerializeField]
    private AvailableAttackDisplay[] displays;

    private void Awake()
    {
        Debug.Log("?");
    }

    public void Clear()
    {
        foreach (var display in displays)
            display.Clear();
    }

    public void SetSigils(IReadOnlyList<Sigil> sigils)
    {
        Clear();
        int count = Mathf.Min(displays.Length, sigils.Count);
        for (int i = 0; i < count; i++)
        {
            displays[i].SetSigil(sigils[i]);
        }
    }

}
