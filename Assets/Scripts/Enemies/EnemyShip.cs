using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    private Vector3 m_moveToPosition;
    private Enemy_Weapon m_weapon;
    private StateMachine m_stateMachine;

    public GameObject m_player { get; private set; }

    #region Enemy States
    public Enemy_IdleState enemyIdleState { get; private set; }
    public Enemy_MoveState enemyMoveState { get; private set; }
    public Enemy_ShootState enemyShootState { get; private set; }
    #endregion

    [Header("Behavior Details")]
    // Idle Details
    [Tooltip("If \"false\", enemy will wait in IdleTimeRange.x [seconds] until start moving.")]
    public bool useRandomIdleTime = true;
    public Vector2 idleTimeRange;
    [Space]
    // Movement Details
    [SerializeField] private float moveSpeed = 5.0f;
    public float acceptableDistanceForShootingPlayer = 0.5f;
    public float waitForSecsForShootingPlayer = 0.25f;
    [Space]
    //[Tooltip("If \"false\", enemy will shoot bullets ShootAmountRange.x [times].")]
    //[SerializeField] private bool useRandomAxisShooting = true;
    public float waitUntilReturnToIdle = 0.25f;

    protected void Awake()
    {
        m_stateMachine = new StateMachine();

        m_player = GameObject.FindWithTag("Player");
        m_weapon = GetComponent<Enemy_Weapon>();

        // Declare enemy's states;
        enemyIdleState = new Enemy_IdleState(this, m_stateMachine);
        enemyMoveState = new Enemy_MoveState(this, m_stateMachine);
        enemyShootState = new Enemy_ShootState(this, m_stateMachine);
    }

    private void OnEnable()
    {
        Player.OnDead += PlayerOnDeadHandler;
    }

    private void OnDisable()
    {
        Player.OnDead -= PlayerOnDeadHandler;
    }

    protected void Start()
    {
        m_stateMachine.Initialize(enemyIdleState);
    }

    protected void Update()
    {
        m_stateMachine.UpdateActiveState();
    }

    public void Move()
    {
        m_moveToPosition = m_player.transform.position;
        m_moveToPosition.z = transform.position.z;

        Vector3 moveToVector = m_moveToPosition - transform.position;
        moveToVector.Normalize();

        transform.Translate(moveToVector * moveSpeed * Time.deltaTime, Space.World);
    }

    public Vector3 GetMoveToPosition() => m_moveToPosition;

    public void Shoot() => m_weapon.Shoot();

    public void PlayerOnDeadHandler()
    {
        m_player = null;
        m_stateMachine.ChangeState(enemyIdleState);
    }
}
