using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shape", menuName = "Scriptable Objects/Shape")]
public class LineShape : ScriptableObject
{
    [SerializeField]
    private Vector2[] points;

    private Vector2[] pointsNormalized;
    public Vector2[] PointsNormalized
    {
        get
        {
            if (pointsNormalized == null)
            {
                pointsNormalized = new Vector2[points.Length];
                ShapesHelper.GetNormalizedPoints(points, pointsNormalized);
            }

            return pointsNormalized;
        }
    }

    [SerializeField]
    private Color color = Color.white;


    [ContextMenu("Normalize")]
    private void Normalize()
    {
        ShapesHelper.GetNormalizedPoints(points, points);
    }
}

public static class ShapesHelper
{
    public static void GetNormalizedPoints(IReadOnlyList<Vector2> points, IList<Vector2> normalizedPoints)
    {
        int count = Mathf.Min(points.Count, normalizedPoints.Count);
        var min = Vector2.positiveInfinity;
        var max = Vector2.negativeInfinity;
        var centroid = new Vector2();
        for (int i = 0; i < count; i++)
        {
            var point = points[i];
            centroid += point;
            max = Vector2.Max(max, point);
            min = Vector2.Min(min, point);
        }

        centroid /= count;
        var size = max - min;
        float extentsValue = Mathf.Max(size.x, size.y) / 2;

        for (int i = 0; i < count; i++)
        {
            var point = points[i];
            point -= centroid;
            point /= extentsValue;
            normalizedPoints[i] = point;
        }
    }
}
