using UnityEngine;

public class BossNexus : Enemy
{
    private BossNexus_Health[] healthComponents;
    private BossNexus_Weapon bossWeapon;
    private int healthComponentCount;
    private float elapsedTime = 0;

    #region Boss States
    public BossNexus_IdleState bossNexus_idleState      { get; private set; }
    public BossNexus_MoveState bossNexus_moveState      { get; private set; }
    public BossNexus_AttackState bossNexus_attackState  { get; private set; }
    #endregion

    [Header("Idle Behavior Details")]
    public bool useRandomIdleTime = true;
    public Vector2 idleTimeRange;

    [Header("Movement Details")]
    [SerializeField] private GameObject bossModelParent;
    [SerializeField] private float rotateSpeed;

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
        //base.Update();

        RotatingShip();
    }

    public override void Move()
    {
        m_moveToPosition = m_player.transform.position;
        m_moveToPosition.z = transform.position.z;

        Vector3 moveToVector = m_moveToPosition - transform.position;
        moveToVector.Normalize();

        transform.Translate(moveToVector * moveSpeed * Time.deltaTime, Space.World);
    }

    private void RotatingShip()
    {
        Vector3 rotatePoint = bossModelParent.transform.localRotation.eulerAngles;
        rotatePoint += new Vector3(0.0f, Time.deltaTime * rotateSpeed, 0.0f);

        bossModelParent.transform.localRotation = Quaternion.Euler(rotatePoint);
    }

    [ContextMenu("Open Position")]
    public void GettingIntoShieldOpenPosition()
    {
        Vector3 newRotatePoint = transform.rotation.eulerAngles + new Vector3(90.0f, 0.0f, 0.0f);
        transform.Rotate(newRotatePoint);
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

    public void Shoot() => bossWeapon.Shoot();

    public void StopShooting() => bossWeapon.StopShooting();

    protected override void PlayerOnDeadHandler()
    {
        //base.PlayerOnDeadHandler();

        m_stateMachine.ChangeState(bossNexus_idleState);
    }
}
