using UnityEngine;

public class Player_Health : Entity_Health
{
    public override void TakeDamage(Collider hitObject)
    {
        base.TakeDamage(hitObject);

        //camera shake
        CameraController.instance.CameraShake();
    }
}
