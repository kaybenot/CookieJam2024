using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shape", menuName = "Scriptable Objects/Shape")]
public class LineShape : ScriptableObject
{
    [SerializeField]
    private Vector2[] points;

    private Vector2[] pointsNormalized;
    public IReadOnlyList<Vector2> PointsNormalized
    {
        get
        {
            if (pointsNormalized == null || pointsNormalized.Length != points.Length)
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

    public static float Distance(LineShape shape, IReadOnlyList<Vector2> line)
    {
        float sqrDistancesSum = 0;
        for (int j = 0; j < line.Count; j++)
        {
            var point = line[j];

            float minSqrDistance = float.MaxValue;
            for (int i = 1; i < shape.PointsNormalized.Count; i++)
            {
                var start = shape.PointsNormalized[i - 1];
                var end = shape.PointsNormalized[i];
                float sqrDistance = Geometry.SqrDistancePointLine(point, start, end);
                if (sqrDistance < minSqrDistance)
                    minSqrDistance = sqrDistance;
            }

            sqrDistancesSum += minSqrDistance;
        }

        return sqrDistancesSum;
    }
}

public static class Geometry
{
    public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 relativePoint = point - lineStart;
        Vector3 lineDirection = lineEnd - lineStart;
        float length = lineDirection.magnitude;
        Vector3 normalizedLineDirection = lineDirection;
        if (length > .000001f)
            normalizedLineDirection /= length;

        float dot = Vector3.Dot(normalizedLineDirection, relativePoint);
        dot = Mathf.Clamp(dot, 0.0F, length);

        return lineStart + normalizedLineDirection * dot;
    }

    public static float SqrDistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        return Vector3.SqrMagnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
    }
}
