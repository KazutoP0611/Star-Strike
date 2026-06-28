using UnityEngine;

public class Bullet_Damager : Entity_Damager
{
    private float destroyTime;
    
    [Header("General Details")]
    [SerializeField] private float bulletSpeed = 30.0f;
    [SerializeField] private float destroyInSecs = 3.0f;

    private void OnEnable()
    {
        destroyTime = Time.time + destroyInSecs;
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;

        if (Time.time >= destroyTime)
            DisableOnDamagable();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Shield>())
        {
            DisableOnDamagable();
            return;
        }

        base.OnTriggerEnter(other);
    }
}
