using UnityEngine;

public class Entity_Damager : MonoBehaviour
{
    private bool m_canDoDamage = true;

    [Header("General Details")]
    [SerializeField] protected Collider col;
    [SerializeField] protected bool move = false;
    [SerializeField] protected bool destroyAfterDidDamage = true;

    [Header("Movement Details")]
    [SerializeField] private float bulletSpeed = 30.0f;

    protected virtual void Update()
    {
        if (move)
            Move();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (m_canDoDamage)
            DoDamage(other);
    }

    protected void Move()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
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
        m_canDoDamage = false;
        Destroy(gameObject);
    }
}
