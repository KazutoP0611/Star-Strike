using UnityEngine;

public class BossNexus_AttackState : BossNexusState
{
    public BossNexus_AttackState(BossNexus bossNexus, StateMachine stateMachine) : base(bossNexus, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        bossNexus.Shoot();
    }

    public override void Update()
    {
        base.Update();

        bossNexus.Move();
    }

    public override void Exit()
    {
        base.Exit();

        bossNexus.StopShooting();
    }
}
