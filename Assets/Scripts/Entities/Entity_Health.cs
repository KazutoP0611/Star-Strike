using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity_Health : MonoBehaviour
{
    private int currentHealth;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    protected Entity_VFX entityVFX;

    [Header("Health Details")]
    [SerializeField] private int maxHealth;

    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnParticleCollision(GameObject other)
    {

        if (currentHealth < 0)
            return;

        currentHealth--;

        // Get Particle Collision, but this can cause a lot of performance.
        // But it will be called only hit event, so I guess right now it is ok.
        ParticleSystem ps = other.GetComponent<ParticleSystem>();
        ps.GetCollisionEvents(gameObject, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++)
        {
            Vector3 hitPosition = collisionEvents[i].intersection;
            entityVFX?.OnDamage(hitPosition);
        }
        //---------------------------------------------------------------------

        if (currentHealth <= 0)
            OnDead();
    }

    protected virtual void OnDead()
    {
        entityVFX?.CreateEffect(ParticleType.Dead);
    }
}
