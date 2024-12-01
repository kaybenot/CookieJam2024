using System.Collections.Generic;
using UnityEngine;

public delegate void ShapePointAddedEventHandler(int pointIndex);

public class LineInstance : MonoBehaviour
{
    public event ShapePointAddedEventHandler OnPointAdded;

    [SerializeField]
    private List<Vector2> linePoints;
    public IReadOnlyList<Vector2> LinePoints => linePoints;

    [SerializeField]
    private List<Vector2> normalizedPoints;
    public List<Vector2> NormalizedPoints => normalizedPoints;

    public void Clear()
    {
        linePoints.Clear();
        normalizedPoints.Clear();
    }

    public void AddLinePoint(Vector2 newPoint)
    {
        linePoints.Add(newPoint);
        normalizedPoints.Add(new Vector2());
        ShapesHelper.GetNormalizedPoints(linePoints, normalizedPoints);

        OnPointAdded?.Invoke(linePoints.Count - 1);
    }

    public void SetPosition(int pointIndex, Vector2 position)
    {
        linePoints[pointIndex] = position;
        ShapesHelper.GetNormalizedPoints(linePoints, normalizedPoints);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 1; i < normalizedPoints.Count; i++)
        {
            var start = normalizedPoints[i - 1];
            var end = normalizedPoints[i];
            Gizmos.DrawLine(start, end);
        }
    }
}
