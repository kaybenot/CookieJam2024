﻿using System.Collections.Generic;
using UnityEngine;

public class PlayerSigilsController : MonoBehaviour
{
    public event System.Action<Sigil> OnSigilDrawn;

    [SerializeField]
    private ShapesDrawingController shapesDrawingController;

    [SerializeField]
    private List<LineInstance> drawnShapes;

    [SerializeField]
    private Player player;

    [SerializeField]
    private float resetCooldown;
    [SerializeField]
    private float timer;

    private void OnEnable()
    {
        shapesDrawingController.OnShapeDrawingStarted += ShapesDrawingController_OnShapeDrawingStarted;
        shapesDrawingController.OnShapeDrawn += ShapesDrawingController_OnShapeDrawn;
    }

    private void ShapesDrawingController_OnShapeDrawingStarted(LineInstance lineShape)
    {
        drawnShapes.Add(lineShape);
    }

    private void ShapesDrawingController_OnShapeDrawn(LineInstance lineShape)
    {
        timer = 0;
    }

    private void Update()
    {
        if (shapesDrawingController.IsDrawing)
            return;
        if (timer > resetCooldown)
            return;

        timer += Time.deltaTime;
        if (timer > resetCooldown)
        {
            CancelChain();
            return;
        }
    }

    private void CancelChain()
    {
        foreach (var line in drawnShapes)
            Destroy(line.gameObject);

        //if (player.TryGetSigil(drawnShapes, out var sigil))
        //    OnSigilDrawn?.Invoke(sigil);

        drawnShapes.Clear();
    }

    private void OnDisable()
    {
        shapesDrawingController.OnShapeDrawn -= ShapesDrawingController_OnShapeDrawn;
        shapesDrawingController.OnShapeDrawingStarted -= ShapesDrawingController_OnShapeDrawingStarted;
    }
}

