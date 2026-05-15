using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class Entity_Health : MonoBehaviour
{
    private int currentHealth;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    protected Entity_VFX entityVFX;
    protected Entity_SFX entitySFX;

    [Header("Health Details")]
    [SerializeField] private int maxHealth;

    private void Start()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entitySFX = GetComponent<Entity_SFX>();

        currentHealth = maxHealth;
    }

    private void OnParticleCollision(GameObject other)
    {
        // Get Particle Collision, but this can cause a lot of performance.
        // But it will be called only hit event, so I guess right now it is ok.
        ParticleSystem ps = other.GetComponent<ParticleSystem>();
        ps.GetCollisionEvents(gameObject, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++)
        {
            Vector3 hitPosition = collisionEvents[i].intersection;

            // Play VFX and SFX
            entityVFX?.OnDamage(hitPosition);
            entitySFX?.PlaySoundAtPoint(SoundType.Damage);
        }
        //---------------------------------------------------------------------

        LostHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            LostHealth();

            entityVFX?.OnDamage(other.gameObject.transform.position);
            entitySFX?.PlaySoundAtPoint(SoundType.Damage);

            Destroy(other.gameObject);
        }
    }

    private void LostHealth()
    {
        if (currentHealth < 0)
            return;

        currentHealth--;

        if (currentHealth <= 0)
            OnDead();
    }

    protected virtual void OnDead()
    {
        entityVFX?.CreateEffect(ParticleType.Dead);
        entitySFX?.PlaySoundAtPoint(SoundType.Destroyed);
    }
}
