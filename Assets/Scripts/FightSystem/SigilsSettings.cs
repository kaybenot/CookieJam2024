using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SigilsSettings : ScriptableObject
{
    [SerializeField]
    private Sigil[] allSigils;

    private Dictionary<IReadOnlyList<LineShape>, Sigil> sigilsByShapes;

    public bool TryGetSigil(IReadOnlyList<LineShape> shapes, out Sigil sigil)
    {
        sigilsByShapes ??= CreateDictionary();
        return sigilsByShapes.TryGetValue(shapes, out sigil);
    }

    private Dictionary<IReadOnlyList<LineShape>, Sigil> CreateDictionary()
    {
        var dict = new Dictionary<IReadOnlyList<LineShape>, Sigil>(new SigilShapeComparer());
        for (int i = 0; i < allSigils.Length; i++)
        {
            var sigil = allSigils[i];
            dict.Add(sigil.Shape, sigil);
        }

        return dict;
    }

    internal class SigilShapeComparer : IEqualityComparer<IReadOnlyList<LineShape>>
    {
        public bool Equals(IReadOnlyList<LineShape> x, IReadOnlyList<LineShape> y)
        {
            int count = x.Count;
            for (int i = 0; i < count; i++)
                if (Contains(y, x[i]) == false)
                    return false;
        
            for (int i = 0; i < count; i++)
                if (Contains(x, y[i]) == false)
                    return false;

            return true;
        }

        public int GetHashCode(IReadOnlyList<LineShape> lineShapes) => lineShapes.Count;
        
        public static bool Contains<T>(IReadOnlyList<T> list, T element)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].Equals(element))
                    return true;

            return false;
        }
    }



}
