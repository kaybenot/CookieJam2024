using UnityEngine;

public class ShapesDrawingController : MonoBehaviour
{
    public event System.Action<LineInstance> OnShapeDrawn;

    [SerializeField]
    private MouseInputEventProvider mouseInputEventProvider;
    [SerializeField]
    private LineInstance shapePrototype;
    [SerializeField]
    private LineShape[] checkedShapes;
    [SerializeField]
    private float minPointsDistance = 0.1f;

    [Space]
    [SerializeField]
    private Vector2 lastPoint;
    public Vector2 LastPoint => lastPoint;

    [SerializeField]
    private LineInstance currentlyDrawnShape;

    public LineShape bestShape;
    public bool IsDrawing { get; private set; }

    private void OnEnable()
    {
        mouseInputEventProvider.OnPressed += MouseInputEventProvider_OnPressed;
        mouseInputEventProvider.OnReleased += MouseInputEventProvider_OnReleased;
    }

    private void MouseInputEventProvider_OnPressed()
    {
        bestShape = null;
        currentlyDrawnShape = Instantiate(shapePrototype, transform);

        currentlyDrawnShape.AddLinePoint(GetCurrentMouseScreenPosition());
    }

    private Vector2 GetCurrentMouseScreenPosition() => Input.mousePosition;

    private void Update()
    {
        IsDrawing = mouseInputEventProvider.IsDragging;
        lastPoint = GetCurrentMouseScreenPosition();
        if (IsDrawing)
        {
            UpdateShape();
        }
    }

    private void UpdateShape()
    {
        float sqrDistance = (lastPoint - currentlyDrawnShape.NormalizedPoints[^1]).sqrMagnitude;
        if (sqrDistance > minPointsDistance * minPointsDistance)
        {
            currentlyDrawnShape.AddLinePoint(lastPoint);
            RecognizeShape();
        }
    }

    private void MouseInputEventProvider_OnReleased()
    {
        RecognizeShape();
        if (bestShape != null)
            OnShapeDrawn?.Invoke(currentlyDrawnShape);
    }

    private void RecognizeShape()
    {
        int bestShapeIndex = 0;
        float bestShapeValue = float.MaxValue;
        for (int i = 0; i < checkedShapes.Length; i++)
        {
            var shape = checkedShapes[i];
            var shapeValue = ShapesHelper.Distance(shape, currentlyDrawnShape.NormalizedPoints);
            Debug.Log($"{checkedShapes[i].name} has distance {shapeValue}");
            if (shapeValue < bestShapeValue)
            {
                bestShapeIndex = i;
                bestShapeValue = shapeValue;
            }
        }

        if (bestShapeValue > 100)
        {
            bestShape = null;
            Debug.Log($"Best shape: NONE");
        }
        else
        {
            bestShape = checkedShapes[bestShapeIndex];
            Debug.Log($"Best shape: {bestShape.name}");
        }
    }


    private void OnDisable()
    {
        mouseInputEventProvider.OnPressed -= MouseInputEventProvider_OnPressed;
        mouseInputEventProvider.OnReleased -= MouseInputEventProvider_OnReleased;
    }


    private void OnDrawGizmos()
    {
        foreach (var shape in checkedShapes)
        {
            Gizmos.color = shape.Color;
            for (int i = 1; i < shape.PointsNormalized.Count; i++)
            {
                var start = shape.PointsNormalized[i - 1];
                var end = shape.PointsNormalized[i];
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
