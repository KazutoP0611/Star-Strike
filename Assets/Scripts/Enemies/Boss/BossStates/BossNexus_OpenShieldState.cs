using UnityEngine;

public class BossNexus_OpenShieldState : BossNexusState
{
    private bool finishedOpenShield;

    public BossNexus_OpenShieldState(BossNexus bossNexus, StateMachine stateMachine) : base(bossNexus, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        bossNexus.SetShieldDamagable(true);
    }

    public override void Update()
    {
        base.Update();

        bossNexus.GettingIntoShieldOpenPosition(out finishedOpenShield);

        if (finishedOpenShield)
            stateMachine.ChangeState(bossNexus.bossNexus_idleState);
    }

    public override void Exit()
    {
        base.Exit();

        bossNexus.SetShieldDamagable(false);
    }
}
