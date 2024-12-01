using System.Collections.Generic;
using UnityEngine;

public class PlayerSigilsController : MonoBehaviour
{
    public event System.Action<Sigil> OnSigilDrawn;

    [SerializeField]
    private ShapesDrawingController shapesDrawingController;

    [SerializeField]
    private List<LineShape> drawnShapes;

    [SerializeField]
    private Player player;

    [SerializeField]
    private float resetCooldown;
    [SerializeField]
    private float timer;

    private void OnEnable()
    {
        shapesDrawingController.OnShapeDrawn += ShapesDrawingController_OnShapeDrawn;
    }

    private void ShapesDrawingController_OnShapeDrawn(LineShape lineShape)
    {
        drawnShapes.Add(lineShape);
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
        if (player.TryGetSigil(drawnShapes, out var sigil))
            OnSigilDrawn?.Invoke(sigil);

        drawnShapes.Clear();
    }

    private void OnDisable()
    {
        shapesDrawingController.OnShapeDrawn -= ShapesDrawingController_OnShapeDrawn;
    }

}

