using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity_Health : MonoBehaviour
{
    private int currentHealth;

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
        entityVFX?.OnDamage();

        if (currentHealth <= 0)
            OnDead();
    }

    protected virtual void OnDead()
    {
        entityVFX?.CreateOnDeadEffect();
    }
}
