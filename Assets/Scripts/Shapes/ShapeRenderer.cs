using UnityEngine;
using UnityEngine.Serialization;

public abstract class ShapeRenderer : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("shapesDrawingController")]
    protected LineInstance shape;
    public LineInstance Shape
    {
        get => shape;
        set
        {
            OnDisable();
            shape = value;
            OnEnable();
        }
    }

    protected virtual void OnEnable()
    {
        if (shape != null)
            shape.OnPointAdded += OnPointAdded;
    }

    protected abstract void OnPointAdded(int pointIndex);

    protected virtual void OnDisable()
    {
        if (shape != null)
            shape.OnPointAdded -= OnPointAdded;
    }
}
