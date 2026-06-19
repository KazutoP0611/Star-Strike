using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected StateMachine m_stateMachine;
    protected Vector3 m_moveToPosition;
    protected bool isDead;

    [Header("Tempolarity Variables")]
    

    public GameObject m_player { get; protected set; }

    protected virtual void Awake()
    {
        m_stateMachine = new StateMachine();

        m_player = GameObject.FindWithTag("Player");
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
        if (isDead)
            return;

        m_stateMachine.UpdateActiveState();
    }

    public virtual void Move() {}

    public void Died() => isDead = true;

    protected virtual void PlayerOnDeadHandler() {}
}
