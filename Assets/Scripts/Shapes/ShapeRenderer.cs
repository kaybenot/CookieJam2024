using UnityEngine;
using UnityEngine.Serialization;

public abstract class ShapeRenderer : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("shapesDrawingController")]
    protected LineInstance shape;

    protected virtual void OnEnable()
    {
        shape.OnPointAdded += OnPointAdded;
    }

    protected abstract void OnPointAdded(int pointIndex);

    protected virtual void OnDisable()
    {
        shape.OnPointAdded -= OnPointAdded;
    }
}
