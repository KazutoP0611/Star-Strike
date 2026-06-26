using UnityEngine;

public class EnemyShip : Enemy
{
    private Enemy_Weapon m_weapon;

    #region Enemy States
    public Enemy_IdleState enemyIdleState   { get; private set; }
    public Enemy_MoveState enemyMoveState   { get; private set; }
    public Enemy_ShootState enemyShootState { get; private set; }
    #endregion

    [Header("Behavior Details")]
    // Idle Details
    [Tooltip("If \"false\", enemy will wait in IdleTimeRange.x [seconds] until start moving.")]
    public bool useRandomIdleTime = true;
    public Vector2 idleTimeRange;
    public Vector2 waitForSecsForShootingPlayer;
    [Space]
    // Movement Details
    public float acceptableDistanceForShootingPlayer = 0.5f;
    
    [Space]
    //[Tooltip("If \"false\", enemy will shoot bullets ShootAmountRange.x [times].")]
    //[SerializeField] private bool useRandomAxisShooting = true;
    public float waitUntilReturnToIdle = 0.25f;

    protected override void Awake()
    {
        base.Awake();

        m_weapon = GetComponent<Enemy_Weapon>();

        // Declare enemy's states;
        enemyIdleState = new Enemy_IdleState(this, m_stateMachine);
        enemyMoveState = new Enemy_MoveState(this, m_stateMachine);
        enemyShootState = new Enemy_ShootState(this, m_stateMachine);
    }

    protected void Start()
    {
        m_stateMachine.Initialize(enemyIdleState);
    }

    public override void Move()
    {
        m_moveToPosition = m_player.transform.position;
        m_moveToPosition.z = transform.position.z;

        Vector3 moveToVector = m_moveToPosition - transform.position;
        moveToVector.Normalize();

        transform.Translate(moveToVector * moveSpeed * Time.deltaTime, Space.World);
    }

    public void Shoot() => m_weapon.Shoot();

    protected override void PlayerOnDeadHandler()
    {
        //m_player = null;
        m_stateMachine.ChangeState(enemyIdleState);
    }

    public float GetWaitShootTime() => Random.Range(waitForSecsForShootingPlayer.x, waitForSecsForShootingPlayer.y);
}
