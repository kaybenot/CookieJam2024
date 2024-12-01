using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SigilsSettings : ScriptableObject
{
    [SerializeField]
    private Sigil[] sigils;

    private Dictionary<IReadOnlyList<LineShape>, Sigil> sigilsByShapes;

    public bool TryGetSigil(IReadOnlyList<LineShape> shapes, out Sigil sigil)
    {
        sigilsByShapes ??= CreateDictionary();
        return sigilsByShapes.TryGetValue(shapes, out sigil);
    }

    private Dictionary<IReadOnlyList<LineShape>, Sigil> CreateDictionary()
    {
        var dict = new Dictionary<IReadOnlyList<LineShape>, Sigil>(new SigilShapeComparer());
        for (int i = 0; i < sigils.Length; i++)
        {
            var sigil = sigils[i];
            dict.Add(sigil.Shape, sigil);
        }

        return dict;
    }
}
