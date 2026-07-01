using UnityEngine;

public class BossNexus_Missile_Health : Entity_Health
{
    public override void TakeDamage(Collider hitObject)
    {
        base.TakeDamage(hitObject);

        if (m_currentHealth <= 0)
        {
            Die();
            Destroy(gameObject);
        }
    }
}
