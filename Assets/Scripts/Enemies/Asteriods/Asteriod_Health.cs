using UnityEngine;

public class Asteriod_Health : Entity_Health
{
    public override void TakeDamage(Collider hitObject)
    {
        base.TakeDamage(hitObject);

        if (m_currentHealth <= 0)
            Die();
    }

    protected override void Die()
    {
        base.Die();

        CameraController.instance.CameraShake();

        Destroy(gameObject);
    }
}
