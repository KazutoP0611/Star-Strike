using UnityEngine;

public class Bullet_Damager : Entity_Damager
{
    private float destroyTime;
    
    [Header("General Details")]
    [SerializeField] private float bulletSpeed = 30.0f;
    [SerializeField] private LayerMask damagableLayer;
    [SerializeField] private float bulletHitDistance = 0.1f;
    [SerializeField] private float destroyInSecs = 3.0f;

    private void OnEnable()
    {
        destroyTime = Time.time + destroyInSecs;
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;

        CheckHit();

        if (Time.time >= destroyTime)
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
