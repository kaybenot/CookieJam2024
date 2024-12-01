using System.Collections.Generic;

public class SigilShapeComparer : IEqualityComparer<IReadOnlyList<LineShape>>
{
    public static SigilShapeComparer Instance { get; } = new SigilShapeComparer();

    public bool Equals(IReadOnlyList<LineShape> x, IReadOnlyList<LineShape> y)
    {
        if (x.Count != y.Count)
            return false;

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
