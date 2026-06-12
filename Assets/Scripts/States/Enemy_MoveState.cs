using UnityEngine;

public class Enemy_MoveState : EnemyState
{
    private bool countingDown = false;

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

        if (enemy.m_player == null)
            return;

        enemy.Move();

        if (IsAligningWithPlayer()/*IsReadyToShoot()*/)
            stateMachine.ChangeState(enemy.enemyShootState);
    }

    //private bool IsReadyToShoot()
    //{
    //    if (IsAligningWithPlayer())
    //    {
    //        if (!countingDown)
    //        {
    //            countingDown = true;
    //            stateTimer = enemy.waitForSecsForShootingPlayer;
    //        }

    //        if (stateTimer <= 0)
    //            return true;
    //    }
    //    else
    //        countingDown = false;

    //    return false;
    //}

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
