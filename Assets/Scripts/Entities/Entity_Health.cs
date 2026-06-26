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
    [Tooltip("Represent how many times this character can take hit.")]
    [SerializeField] protected int maxHealth;

    protected virtual void Awake()
    {
        m_entityVFX = GetComponent<Entity_VFX>();
        m_entitySFX = GetComponent<Entity_SFX>();

        m_currentHealth = maxHealth;

        //Initialize();
    }

    //protected virtual void Initialize() { }

    public virtual void TakeDamage(Collider hitObject)
    {
        LostHealth();

        // Create effect and play sound effect at hit point (sort of)
        m_entityVFX?.OnDamage(hitObject.transform.position);
        m_entitySFX?.PlaySoundAtPoint(SoundType.Damage);

        //Destroy(hitObject.gameObject);
    }

    protected virtual void LostHealth()
    {
        if (m_currentHealth < 0)
            return;

        m_currentHealth--;

        //if (m_currentHealth <= 0)
        //    Die();
    }

    protected virtual void Die()
    {
        m_entityVFX?.CreateEffect(ParticleType.Dead);
        m_entitySFX?.PlaySoundAtPoint(SoundType.Destroyed);
    }
}
