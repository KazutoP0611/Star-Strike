using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    private Vector3 moveToPosition;
    private Vector3 moveToVector;

    private GameObject player;
    private StateMachine stateMachine;

    #region Enemy States
    public Enemy_IdleState enemyIdleState { get; private set; }
    public Enemy_MoveState enemyMoveState { get; private set; }
    #endregion

    [Header("Behavior Details")]
    public bool useRandomIdleTime = true;
    public Vector2 idleTimeRange;
    [SerializeField] private float moveSpeed = 5.0f;
    public float acceptableDistanceForShootingPlayer = 0.5f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");

        stateMachine = new StateMachine();

        // Declare enemy's states;
        enemyIdleState = new Enemy_IdleState(this, stateMachine);
        enemyMoveState = new Enemy_MoveState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.Initialize(enemyIdleState);
    }

    private void Update()
    {
        stateMachine.UpdateActiveState();
    }

    public void RegisterMoveVector()
    {
        moveToPosition = player.transform.position;
        moveToPosition.z = transform.position.z;

        moveToVector = moveToPosition - transform.position;
        moveToVector.Normalize();
    }

    public void Move()
    {
        transform.Translate(moveToVector * moveSpeed * Time.deltaTime, Space.World);
    }

    public Vector3 GetMoveToPosition() => moveToPosition;
}
