using UnityEngine;

public class ShapesDrawingController : MonoBehaviour
{
    public event System.Action<LineInstance> OnShapeDrawingStarted;
    public event System.Action<LineInstance> OnShapeDrawn;

    [SerializeField]
    private MouseInputEventProvider mouseInputEventProvider;
    [SerializeField]
    private LineInstance shapePrototype;
    [SerializeField]
    private float minPointsDistance = 0.1f;

    [Space]
    [SerializeField]
    private Vector2 lastPoint;
    public Vector2 LastPoint => lastPoint;

    [SerializeField]
    private LineInstance currentlyDrawnShape;

    public LineShape bestShape;

    [field:SerializeField]
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
        OnShapeDrawingStarted?.Invoke(currentlyDrawnShape);
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
        }
    }

    private void MouseInputEventProvider_OnReleased()
    {
        OnShapeDrawn?.Invoke(currentlyDrawnShape);
    }

    private void OnDisable()
    {
        mouseInputEventProvider.OnPressed -= MouseInputEventProvider_OnPressed;
        mouseInputEventProvider.OnReleased -= MouseInputEventProvider_OnReleased;
    }

}
