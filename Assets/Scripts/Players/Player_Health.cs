using UnityEngine;
using UnityEngine.UI;

public class Player_Health : Entity_Health
{
    private Player player;
    private bool isImmortal;
    private float immortalTime;

    [SerializeField] private PlayerHealthBar playerHealthBar;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    private void Start()
    {
        Initialize();
        isImmortal = false;
    }

    private void Update()
    {
        if (isImmortal == false)
            return;

        immortalTime += Time.deltaTime;

        if (immortalTime >= player.immortalTime)
            isImmortal = false;
    }

    protected /*override*/ void Initialize()
    {
        UpdateHealh();
    }

    public override void TakeDamage(Collider hitObject)
    {
        if (isImmortal)
            return;

        base.TakeDamage(hitObject);

        UpdateHealh();
        CameraController.instance.CameraShake();
        isImmortal = true;
        immortalTime = 0;

        if (m_currentHealth <= 0)
            Die();
    }

    private void AddHealth(int value)
    {
        m_currentHealth += value;
        m_currentHealth = Mathf.Clamp(m_currentHealth, 0, maxHealth);

        UpdateHealh();
    }

    private void UpdateHealh()
    {
        float hpRatio = (float)m_currentHealth / maxHealth;
        playerHealthBar.UpdateHealthBar(hpRatio);
    }

    protected override void Die()
    {
        base.Die();

        player.PlayerStartDying();
    }
}
