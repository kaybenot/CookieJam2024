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
    [SerializeField]
    private PlayerSigilsController playerSigilsController;

    private ObjectPool<ShapeRenderer> renderersPool;

    private readonly Dictionary<LineInstance, ShapeRenderer> renderersByLineInstance = new Dictionary<LineInstance, ShapeRenderer>();

    private void Awake()
    {
        renderersPool = new ObjectPool<ShapeRenderer>(CreateRenderer, rend => rend.enabled = true, rend => rend.enabled = false);
    }

    private void Update()
    {
        if (shapesDrawingController.CurrentlyDrawnShape && renderersByLineInstance.TryGetValue(shapesDrawingController.CurrentlyDrawnShape, out var currentRenderer))
        {
            var targetColor = playerSigilsController.BestShape
                ? playerSigilsController.BestShape.Color
                : new Color(0.7f, 0.7f, 0.7f);
            currentRenderer.Color = Color.Lerp(currentRenderer.Color, targetColor , 0.5f);
        }
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
        line.OnLineDestroyed += Line_OnLineDestroyed;
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
