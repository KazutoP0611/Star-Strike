using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class Entity_Health : MonoBehaviour, IDamagable
{
    protected int m_currentHealth;

    protected Entity_VFX m_entityVFX;
    protected Entity_SFX m_entitySFX;

    [Header("Health Details")]
    [SerializeField] protected int maxHealth;

    protected virtual void Awake()
    {
        m_entityVFX = GetComponent<Entity_VFX>();
        m_entitySFX = GetComponent<Entity_SFX>();

        m_currentHealth = maxHealth;

        Initialize();
    }

    protected virtual void Initialize() {}

    //private void OnParticleCollision(GameObject other)
    //{
    //    // Get Particle Collision, but this can cause a lot of performance.
    //    // But it will be called only hit event, so I guess right now it is ok.
    //    ParticleSystem ps = other.GetComponent<ParticleSystem>();
    //    ps.GetCollisionEvents(gameObject, collisionEvents);

    //    for (int i = 0; i < collisionEvents.Count; i++)
    //    {
    //        Vector3 hitPosition = collisionEvents[i].intersection;

    //        // Play VFX and SFX
    //        entityVFX?.OnDamage(hitPosition);
    //        entitySFX?.PlaySoundAtPoint(SoundType.Damage);
    //    }
    //    //---------------------------------------------------------------------

    //    LostHealth();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Bullet"))
    //    {
    //        TakeDamage();
    //    }
    //}

    public virtual void TakeDamage(Collider hitObject)
    {
        LostHealth();

        // Create effect and play sound effect at hit point (sort of)
        m_entityVFX?.OnDamage(hitObject.transform.position);
        m_entitySFX?.PlaySoundAtPoint(SoundType.Damage);

        //Destroy(hitObject.gameObject);
    }

    private void LostHealth()
    {
        if (m_currentHealth < 0)
            return;

        m_currentHealth--;

        if (m_currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        m_entityVFX?.CreateEffect(ParticleType.Dead);
        m_entitySFX?.PlaySoundAtPoint(SoundType.Destroyed);
    }
}
