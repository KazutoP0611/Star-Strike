using System;
using UnityEngine;

public class BossNexus_Health : Entity_Health
{
    private Collider col;

    private event Action onHealthRunOut;

    protected override void Awake()
    {
        base.Awake();

        col = GetComponent<Collider>();
    }

    public void Initialize(Action onHealthRunOut)
    {
        this.onHealthRunOut = onHealthRunOut;
        ShowHittable(false);
    }

    protected override void LostHealth()
    {
        if (m_currentHealth <= 0)
            return;

        m_currentHealth--;

        if (m_currentHealth <= 0)
        {
            ShowHittable(false);

            //play destroyed sound
            Die();

            //callback to boss ship, tell them that one of health component is destroyed;
            onHealthRunOut?.Invoke();
        }
    }

    public void ShowHittable(bool active)
    {
        col.enabled = active;
        m_entityVFX.SetActiveEmission(active);
    }
}
