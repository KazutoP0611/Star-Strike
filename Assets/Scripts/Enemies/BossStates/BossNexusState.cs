using UnityEngine;

public class BossNexusState : EntityState
{
    protected BossNexus bossNexus;

    public BossNexusState(BossNexus bossNexus, StateMachine stateMachine) : base(stateMachine)
    {
        this.bossNexus = bossNexus;
    }
}
