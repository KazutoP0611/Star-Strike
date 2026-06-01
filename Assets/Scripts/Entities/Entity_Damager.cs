using UnityEngine;

public class Entity_Damager : MonoBehaviour
{
    [SerializeField] protected Collider collider;

    private void OnTriggerEnter(Collider other)
    {
        DoDamage(other);
    }

    protected void DoDamage(Collider other)
    {
        IDamagable damagable = other.GetComponentInParent<IDamagable>();
        damagable?.TakeDamage(collider);

        Destroy(gameObject);
    }
}
