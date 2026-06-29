using UnityEngine;

public class Bullet_Damager : Entity_Damager
{
    private float m_destroyTime;
    
    [Header("Damage Details")]
    [SerializeField] private LayerMask damagableLayer;
    [SerializeField] private float bulletHitDistance = 0.1f;
    [SerializeField] private float destroyInSecs = 3.0f;

    private void OnEnable()
    {
        m_destroyTime = Time.time + destroyInSecs;
    }

    protected override void Update()
    {
        Move();

        CheckHit();

        if (Time.time >= m_destroyTime)
            DisableOnDamagable();
    }

    private void CheckHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletHitDistance, damagableLayer))
        {
            Shield shield = hit.collider.gameObject.GetComponentInParent<Shield>();
            if (shield != null)
            {
                DisableOnDamagable();
                return;
            }

            IDamagable damage = hit.collider.gameObject.GetComponentInParent<IDamagable>();
            if (damage != null)
            {
                damage.TakeDamage(col);
                DisableOnDamagable();
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //if (other.GetComponent<Shield>())
        //{
        //    DisableOnDamagable();
        //    return;
        //}

        //base.OnTriggerEnter(other);
    }
}
