using UnityEngine;

public class Boss_Health : Entity_Health
{
    protected override void Die()
    {
        base.Die();

        CameraController.instance.CameraShake();

        UI_Manager.instance.SetActiveGameOverScreen(true);
    }
}
