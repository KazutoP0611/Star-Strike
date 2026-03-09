using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity_Health : MonoBehaviour
{
    private int currentHealth;

    [Header("Health Details")]
    [SerializeField] private int maxHealth;

    [Header("Particle Details")]
    [SerializeField] private GameObject onDestroyParticle;
    [SerializeField] private float onDestroyParticleScale = 0.75f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (currentHealth < 0)
            return;

        currentHealth--;

        if (currentHealth <= 0)
            OnDead();
    }

    protected virtual void OnDead()
    {
        GameObject explodeParticle = Instantiate(onDestroyParticle, transform.position, transform.localRotation);
        explodeParticle.transform.localScale = Vector3.one * onDestroyParticleScale;
    }
}
