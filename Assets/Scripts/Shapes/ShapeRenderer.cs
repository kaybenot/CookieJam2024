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

    public abstract Color Color { get; set; }

    protected virtual void OnEnable()
    {
        if (shape)
        {
            shape.OnPointAdded += OnPointAdded;
        }
    }

    protected abstract void OnPointAdded(int pointIndex);

    protected virtual void OnDisable()
    {
        if (shape)
        {
            shape.OnPointAdded -= OnPointAdded;
        }
    }
}
