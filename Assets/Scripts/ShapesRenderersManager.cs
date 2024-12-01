using System;
using System.Collections.Generic;
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

    private readonly Dictionary<LineInstance, ShapeRenderer> renderersByLineInstance = new Dictionary<LineInstance, ShapeRenderer>();


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
        renderersByLineInstance.Add(line, renderer);
        line.OnLineDestroyed += Line_OnLineDestroyed; ;
    }

    private void Line_OnLineDestroyed(LineInstance line)
    {
        var renderer = renderersByLineInstance[line];
        renderersByLineInstance.Remove(line);
        renderer.Shape = null;
        renderer.enabled = false;
        renderersPool.Release(renderer);
    }

    private ShapeRenderer CreateRenderer() => Instantiate(shapeRendererPrototype, container);

    private void OnDisable()
    {
        shapesDrawingController.OnShapeDrawingStarted -= ShapesDrawingController_OnShapeDrawingStarted;
    }
}
