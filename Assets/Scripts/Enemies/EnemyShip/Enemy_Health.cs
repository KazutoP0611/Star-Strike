using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private EnemyShip enemy;

    [SerializeField] private GameObject model;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<EnemyShip>();
    }

    public override void TakeDamage(Collider hitObject)
    {
        base.TakeDamage(hitObject);

        //maybe add hurt state?
        //play some animations? something like that.

        if (m_currentHealth <= 0)
            Die();
    }

    protected override void Die()
    {
        base.Die();

        enemy.Died();

        CameraController.instance.CameraShake();

        Destroy(gameObject);
    }
}
