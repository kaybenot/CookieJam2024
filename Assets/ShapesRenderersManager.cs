using System;
using UnityEngine;
using UnityEngine.Pool;

public class ShapesRenderersManager : MonoBehaviour
{
    [SerializeField]
    private ShapesDrawingController shapesDrawingController;
    [SerializeField]
    private Transform container;
    [SerializeField]
    private ShapeRenderer shapeRendererPrototype;

    private ObjectPool<ShapeRenderer> renderersPool;

    private void Awake()
    {
        renderersPool = new ObjectPool<ShapeRenderer>(CreateRenderer, rend => rend.enabled = true, rend => rend.enabled = false);
    }

    private void OnEnable()
    {
        shapesDrawingController.OnShapeDrawingStarted += ShapesDrawingController_OnShapeDrawingStarted;
    }

    private void ShapesDrawingController_OnShapeDrawingStarted(LineInstance line)
    {
        var renderer = renderersPool.Get();
        renderer.Shape = line;
    }

    private ShapeRenderer CreateRenderer() => Instantiate(shapeRendererPrototype, container);

    private void OnDisable()
    {
        shapesDrawingController.OnShapeDrawingStarted -= ShapesDrawingController_OnShapeDrawingStarted;
    }
}
