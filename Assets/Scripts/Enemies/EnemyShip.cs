using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    private Vector3 moveToPosition;

    private GameObject player;
    private StateMachine stateMachine;

    private Enemy_Weapon weapon;

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
    [Tooltip("If \"false\", enemy will shoot bullets ShootAmountRange.x [times].")]
    public bool useRandomAxisShooting = true;
    public float waitUntilReturnToIdle = 0.25f;
    public float shootInterval;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");

        stateMachine = new StateMachine();

        weapon = GetComponent<Enemy_Weapon>();

        // Declare enemy's states;
        enemyIdleState = new Enemy_IdleState(this, stateMachine);
        enemyMoveState = new Enemy_MoveState(this, stateMachine);
        enemyShootState = new Enemy_ShootState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.Initialize(enemyIdleState);
    }

    private void Update()
    {
        stateMachine.UpdateActiveState();
    }

    public void Move()
    {
        moveToPosition = player.transform.position;
        moveToPosition.z = transform.position.z;

        Vector3 moveToVector = moveToPosition - transform.position;
        moveToVector.Normalize();

        transform.Translate(moveToVector * moveSpeed * Time.deltaTime, Space.World);
    }

    public Vector3 GetMoveToPosition() => moveToPosition;

    public void Shoot() => weapon.Shoot();
}
