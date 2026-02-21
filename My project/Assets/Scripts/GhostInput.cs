using UnityEngine;
using UnityEngine.InputSystem;

public class GhostInput : MonoBehaviour
{
    private AstralProjection astralProjection;

    public void Init(AstralProjection ap)
    {
        astralProjection = ap;
    }

    public void OnToggleGhost(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (astralProjection != null)
            astralProjection.RecallToGhost();
    }
}