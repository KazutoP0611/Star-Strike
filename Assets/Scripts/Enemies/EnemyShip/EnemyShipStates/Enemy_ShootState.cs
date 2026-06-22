using UnityEngine;

public enum EnemyShootingAxis
{
    Horizontal,
    Vertical
}

public class Enemy_ShootState : EnemyState
{
    private float shootCountdown;
    private bool hasShot;

    public Enemy_ShootState(EnemyShip enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        hasShot = false;
        shootCountdown = enemy.waitForSecsForShootingPlayer;
    }

    public override void Update()
    {
        base.Update();

        CheckShoot();

        if (hasShot && stateTimer <= 0)
            stateMachine.ChangeState(enemy.enemyIdleState);
    }

    private void CheckShoot()
    {
        shootCountdown -= Time.deltaTime;
        if (shootCountdown <= 0 && !hasShot)
        {
            enemy.Shoot();

            hasShot = true;
            stateTimer = enemy.waitUntilReturnToIdle;
        }
    }
}
