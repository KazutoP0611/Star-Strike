using UnityEngine;

public class Enemy_IdleState : EnemyState
{
    public Enemy_IdleState(EnemyShip enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.useRandomIdleTime ? Random.Range((int)enemy.idleTimeRange.x, (int)enemy.idleTimeRange.y) : (int)enemy.idleTimeRange.x;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.enemyMoveState);
    }
}
