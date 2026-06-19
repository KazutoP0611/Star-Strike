using System;
using UnityEngine;

public class BossNexus_Health : Entity_Health
{
    private event Action onHealthRunOut;

    public void Initialize(Action onHealthRunOut)
    {
        this.onHealthRunOut = onHealthRunOut;
    }

    public override void TakeDamage(Collider hitObject)
    {
        base.TakeDamage(hitObject);
    }

    protected override void LostHealth()
    {
        if (m_currentHealth <= 0)
            return;

        m_currentHealth--;

        if (m_currentHealth <= 0)
        {
            //play destroyed sound
            Die();

            //callback to boss ship, tell them that one of health component is destroyed;
            onHealthRunOut?.Invoke();
        }
    }
}
