using UnityEngine;

public class Entity_Damager : MonoBehaviour
{
    [SerializeField] protected Collider col;
    [SerializeField] protected bool destroyAfterCollided = true;

    private void OnTriggerEnter(Collider other)
    {
        DoDamage(other);
    }

    protected void DoDamage(Collider other)
    {
        IDamagable damagable = other.GetComponentInParent<IDamagable>();
        damagable?.TakeDamage(col);

        if (destroyAfterCollided)
            Destroy(gameObject);
    }
}
