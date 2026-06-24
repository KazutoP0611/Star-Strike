using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected StateMachine m_stateMachine;
    protected Vector3 m_moveToPosition;

    protected bool isDead { get; private set; }
    public Player m_player { get; private set; }

    [SerializeField] protected float moveSpeed = 5.0f;

    protected virtual void Awake()
    {
        m_stateMachine = new StateMachine();

        m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    protected void OnEnable()
    {
        Player.OnDead += PlayerOnDeadHandler;
        isDead = false;
    }

    protected void OnDisable()
    {
        Player.OnDead -= PlayerOnDeadHandler;
    }

    protected virtual void Update()
    {
        if (m_player.IsDead)
            return;

        m_stateMachine.UpdateActiveState();
    }

    public virtual void Move() {}

    public void Died() => isDead = true;

    protected virtual void PlayerOnDeadHandler() {}

    public Vector3 GetMoveToPosition() => m_moveToPosition;
}
