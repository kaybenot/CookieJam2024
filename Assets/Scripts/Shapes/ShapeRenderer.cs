using UnityEngine;

public abstract class ShapeRenderer : MonoBehaviour
{
    [SerializeField]
    protected ShapesDrawingController shapesDrawingController;

    protected virtual void OnEnable()
    {
        shapesDrawingController.OnPointAdded += OnPointAdded;
    }

    protected abstract void OnPointAdded(int pointIndex);

    protected virtual void OnDisable()
    {
        shapesDrawingController.OnPointAdded -= OnPointAdded;
    }
}
