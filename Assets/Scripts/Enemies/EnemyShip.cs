using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    private Vector3 m_moveToPosition;

    private GameObject m_player;
    private StateMachine m_stateMachine;

    private Enemy_Weapon m_weapon;

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

    private void Awake()
    {
        m_player = GameObject.FindWithTag("Player");

        m_stateMachine = new StateMachine();

        m_weapon = GetComponent<Enemy_Weapon>();

        // Declare enemy's states;
        enemyIdleState = new Enemy_IdleState(this, m_stateMachine);
        enemyMoveState = new Enemy_MoveState(this, m_stateMachine);
        enemyShootState = new Enemy_ShootState(this, m_stateMachine);
    }

    private void Start()
    {
        m_stateMachine.Initialize(enemyIdleState);
    }

    private void Update()
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
}
