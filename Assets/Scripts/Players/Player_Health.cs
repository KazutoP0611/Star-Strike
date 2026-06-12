using UnityEngine;
using UnityEngine.UI;

public class Player_Health : Entity_Health
{
    private Player player;

    [SerializeField] private Slider hpBar;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    protected override void Initialize()
    {
        UpdateHealthBar();
    }

    public override void TakeDamage(Collider hitObject)
    {
        //camera shake
        CameraController.instance.CameraShake();

        base.TakeDamage(hitObject);

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float hpRatio = (float)m_currentHealth / maxHealth;
        hpBar.value = hpRatio;
    }

    protected override void Die()
    {
        base.Die();

        player.PlayerStartDying();
    }
}
