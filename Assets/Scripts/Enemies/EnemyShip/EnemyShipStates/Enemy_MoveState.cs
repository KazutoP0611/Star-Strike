using UnityEngine;

public class Enemy_MoveState : EnemyState
{
    public Enemy_MoveState(EnemyShip enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.Move();

        if (IsInShootRange())
            stateMachine.ChangeState(enemy.enemyShootState);
    }

    private bool IsInShootRange()
    {
        if (IsPlayerInFront() == false)
            return false;

        if (GetShootingDistance() < enemy.acceptableDistanceForShootingPlayer)
            return true;

        return false;
    }

    private bool IsPlayerInFront()
    {
        //Vector3 vectorToPlayer = enemy.m_player.transform.position - enemy.transform.position;

        //if (Vector3.Dot(enemy.transform.forward, vectorToPlayer) > enemy.acceptablePlayerDotValue)
        //    return true;

        return enemy.transform.position.z > enemy.m_player.transform.position.z;
    }

    private float GetShootingDistance()
    {
        return Vector3.Distance(enemy.GetMoveToPosition(), enemy.transform.position);
    }
}
