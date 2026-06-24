using UnityEngine;

public class BossNexus : Enemy
{
    private BossNexus_Health[] healthComponents;
    private BossNexus_Weapon bossWeapon;
    private int healthComponentCount;
    private float openShieldAngle = 0;
    private float lastGetPlayerPosTime;

    #region Boss States
    public BossNexus_IdleState bossNexus_idleState              { get; private set; }
    public BossNexus_MoveState bossNexus_moveState              { get; private set; }
    public BossNexus_AttackState bossNexus_attackState          { get; private set; }
    public BossNexus_OpenShieldState bossNexus_openShieldState  { get; private set; }
    #endregion

    [Header("Idle Behavior Details")]
    public bool useRandomIdleTime = true;
    public Vector2 idleTimeRange;

    [Header("Movement Details")]
    [SerializeField] private GameObject bossModelParent;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float getPlayerPositionDelay = 2.0f;

    [Header("Attack Details")]
    public float attackAcceptableRange = 0.2f;
    [SerializeField] private Vector2 shootDuration;

    [Header("Open Shield Details")]
    [SerializeField] private float openRotateSpeed = 10.0f;

    protected override void Awake()
    {
        base.Awake();

        #region Prepare Health Component
        // ----------------- Prepare health components -----------------
        healthComponents = GetComponentsInChildren<BossNexus_Health>();
        healthComponentCount = healthComponents.Length;
        // -------------------------------------------------------------
        #endregion

        #region Prepare Boss States
        // Declare enemy's states;
        bossNexus_idleState = new BossNexus_IdleState(this, m_stateMachine);
        bossNexus_moveState = new BossNexus_MoveState(this, m_stateMachine);
        bossNexus_attackState = new BossNexus_AttackState(this, m_stateMachine);
        bossNexus_openShieldState = new BossNexus_OpenShieldState(this, m_stateMachine);
        #endregion

        bossWeapon = GetComponentInChildren<BossNexus_Weapon>();
    }

    private void Start()
    {
        m_stateMachine.Initialize(bossNexus_idleState);

        // Set up health component callback;
        foreach (var health in healthComponents)
        {
            health.Initialize(HealthLostCallback);
        }
    }

    protected override void Update()
    {
        if (isDead)
            return;

        base.Update();

        RotatingShip();

        //GettingIntoShieldOpenPosition();
    }

    public override void Move()
    {
        if (lastGetPlayerPosTime < Time.time - getPlayerPositionDelay)
        {
            lastGetPlayerPosTime = Time.time;

            m_moveToPosition = m_player.transform.position;
            m_moveToPosition.z = transform.position.z;
        }

        Vector3 moveToPosition = Vector3.Slerp(transform.position, m_moveToPosition, moveSpeed * Time.deltaTime);
        transform.position = moveToPosition;
    }

    private void RotatingShip()
    {
        Vector3 rotatePoint = bossModelParent.transform.localRotation.eulerAngles;
        rotatePoint += new Vector3(0.0f, Time.deltaTime * rotateSpeed, 0.0f);

        bossModelParent.transform.localRotation = Quaternion.Euler(rotatePoint);
    }

    [ContextMenu("Open Position")]
    public void GettingIntoShieldOpenPosition(out bool finishOpenShield)
    {
        openShieldAngle += Time.deltaTime * openRotateSpeed;

        if (openShieldAngle < 360)
        {
            finishOpenShield = false;
        }
        else
        {
            openShieldAngle = 0;
            finishOpenShield = true;
        }

        transform.rotation = Quaternion.Euler(new Vector3(openShieldAngle, 0.0f, 0.0f));
    }

    private void HealthLostCallback()
    {
        CameraController.instance.CameraShake();

        healthComponentCount--;

        if (healthComponentCount <= 0)
        {
            // Maybe start animation sequence or something;
            Died();

            // Show game over screen, well just placeholder for now;
            UI_Manager.instance.SetActiveGameOverScreen(true);
        }
    }

    public void SetShieldDamagable(bool damagable)
    {
        foreach (var health in healthComponents)
        {
            if (health.IsDestroyed == false)
                health.ShowHittable(damagable);
        }
    }

    public void Shoot() => bossWeapon.Shoot();

    public void StopShooting() => bossWeapon.StopShooting();

    protected override void PlayerOnDeadHandler()
    {
        //base.PlayerOnDeadHandler();

        m_stateMachine.ChangeState(bossNexus_idleState);
    }

    public float GetShootingDuration() => Random.Range(shootDuration.x, shootDuration.y);
}
