using UnityEngine;

public class BossNexus_AttackState : BossNexusState
{
    //private float shootDuration;

    public BossNexus_AttackState(BossNexus bossNexus, StateMachine stateMachine) : base(bossNexus, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = bossNexus.GetShootingDuration();
        bossNexus.Shoot();
    }

    public override void Update()
    {
        base.Update();

        bossNexus.Move();

        if (stateTimer < 0)
            stateMachine.ChangeState(bossNexus.bossNexus_openShieldState);
    }

    public override void Exit()
    {
        base.Exit();

        bossNexus.StopShooting();
    }
}
