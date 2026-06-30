using UnityEngine;

public class BossNexus_IdleState : BossNexusState
{
    public BossNexus_IdleState(BossNexus bossNexus, StateMachine stateMachine) : base(bossNexus, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        bossNexus.SetActivateMissle(false);
        stateTimer = bossNexus.useRandomIdleTime ? Random.Range(bossNexus.idleTimeRange.x, bossNexus.idleTimeRange.y) : bossNexus.idleTimeRange.x;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(bossNexus.bossNexus_moveState);
    }
}
