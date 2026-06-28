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
        shootCountdown = enemy.GetWaitShootTime();
    }

    public override void Update()
    {
        //base.Update();

        CheckShoot();

        if (hasShot == false)
            return;

        {
            stateTimer -= Time.deltaTime;

            if (stateTimer <= 0)
                stateMachine.ChangeState(enemy.enemyIdleState);
        }
    }

    private void CheckShoot()
    {
        if (hasShot == true)
            return;

        shootCountdown -= Time.deltaTime;
        if (shootCountdown <= 0)
        {
            enemy.Shoot();

            hasShot = true;
            stateTimer = enemy.waitUntilReturnToIdle;
        }
    }
}
