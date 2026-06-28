using UnityEngine;

public class Entity_Damager : MonoBehaviour
{
    private bool canDoDamage = true;

    [SerializeField] protected Collider col;
    [SerializeField] protected bool destroyAfterDidDamage = true;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (canDoDamage)
            DoDamage(other);
    }

    protected void DoDamage(Collider other)
    {
        IDamagable damagable = other.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.TakeDamage(col);

            if (destroyAfterDidDamage)
                DisableOnDamagable();
        }
    }

    protected void DisableOnDamagable()
    {
        canDoDamage = false;
        Destroy(gameObject);
    }
}
