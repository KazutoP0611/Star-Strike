using UnityEngine;

public class Enemy_MoveState : EnemyState
{
    public Enemy_MoveState(EnemyShip enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Todo
        // Register player's position
        enemy.RegisterMoveVector();
    }

    public override void Update()
    {
        base.Update();

        enemy.Move();

        if (IsAligningWithPlayer())
            stateMachine.ChangeState(enemy.enemyIdleState);
    }

    private bool IsAligningWithPlayer()
    {
        if (GetShootingDistance() < enemy.acceptableDistanceForShootingPlayer)
            return true;

        return false;
    }

    private float GetShootingDistance()
    {
        return Vector3.Distance(enemy.GetMoveToPosition(), enemy.transform.position);
    }
}
