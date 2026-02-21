using UnityEngine;

public class GhostCleanup : MonoBehaviour
{
    private AstralProjection astralProjection;

    public void Init(AstralProjection ap)
    {
        astralProjection = ap;
    }

    private void OnDestroy()
    {
        if (astralProjection != null)
            astralProjection.RecallToGhost();
    }
}