using UnityEngine;
using UnityEngine.UI;

public class Player_Health : Entity_Health
{
    private Player player;
    private bool isImmortal;
    private float immortalTime;

    [SerializeField] private Slider hpBar;

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
        UpdateHealthBar();
    }

    public override void TakeDamage(Collider hitObject)
    {
        if (isImmortal)
            return;

        base.TakeDamage(hitObject);

        UpdateHealthBar();
        CameraController.instance.CameraShake();
        isImmortal = true;
        immortalTime = 0;

        if (m_currentHealth <= 0)
            Die();
    }

    private void UpdateHealthBar()
    {
        float hpRatio = (float)m_currentHealth / maxHealth;
        hpBar.value = hpRatio;

        // Todo
        // Maybe => check hp and show warning according to hp level?;
    }

    protected override void Die()
    {
        base.Die();

        player.PlayerStartDying();
    }
}
