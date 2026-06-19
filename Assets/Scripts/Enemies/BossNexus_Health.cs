using UnityEngine;

public class BossNexus_Health : Entity_Health
{
    public override void TakeDamage(Collider hitObject)
    {
        base.TakeDamage(hitObject);
    }

    //protected override void Die()
    //{
    //    base.Die();

    //    CameraController.instance.CameraShake();

    //    UI_Manager.instance.SetActiveGameOverScreen(true);
    //}
}
