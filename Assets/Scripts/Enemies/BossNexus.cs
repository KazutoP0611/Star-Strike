using UnityEngine;

public class BossNexus : Enemy
{
    private BossNexus_Health[] healthComponents;
    private int healthComponentCount;

    #region Boss States
    public BossNexus_IdleState bossNexus_idleState      { get; private set; }
    public BossNexus_MoveState bossNexus_moveState      { get; private set; }
    public BossNexus_AttackState bossNexus_attackState  { get; private set; }
    #endregion

    [Header("Behavior Details")]
    public bool useRandomIdleTime = true;
    public Vector2 idleTimeRange;

    [Header("Shooting Details")]
    [SerializeField] private GameObject shootingParent;

    protected override void Awake()
    {
        base.Awake();

        #region Prepare Health Component
        // Prepare health components-----------------------------------
        healthComponents = GetComponentsInChildren<BossNexus_Health>();
        healthComponentCount = healthComponents.Length;

        foreach (var health in healthComponents)
        {
            health.Initialize(HealthLostCallback);
        }
        //-------------------------------------------------------------
        #endregion

        #region Prepare Boss States
        // Declare enemy's states;
        bossNexus_idleState = new BossNexus_IdleState(this, m_stateMachine);
        bossNexus_moveState = new BossNexus_MoveState(this, m_stateMachine);
        bossNexus_attackState = new BossNexus_AttackState(this, m_stateMachine);
        #endregion
    }

    private void Start()
    {
        m_stateMachine.Initialize(bossNexus_idleState);
    }

    public override void Move()
    {
        m_moveToPosition = m_player.transform.position;
        m_moveToPosition.z = transform.position.z;

        Vector3 moveToVector = m_moveToPosition - transform.position;
        moveToVector.Normalize();

        transform.Translate(moveToVector * moveSpeed * Time.deltaTime, Space.World);
    }

    private void HealthLostCallback()
    {
        CameraController.instance.CameraShake();
        healthComponentCount--;

        if (healthComponentCount <= 0)
        {
            // Maybe start animation sequence or something;

            // Show game over screen, well just placeholder for now;
            UI_Manager.instance.SetActiveGameOverScreen(true);
        }
    }

    public void Shoot() => shootingParent.SetActive(true);
}
