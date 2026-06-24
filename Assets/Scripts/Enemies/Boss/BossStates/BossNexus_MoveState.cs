using UnityEngine;

public class BossNexus_MoveState : BossNexusState
{
    public BossNexus_MoveState(BossNexus bossNexus, StateMachine stateMachine) : base(bossNexus, stateMachine)
    {
    }

    public override void Update()
    {
        base.Update();

        bossNexus.Move();

        if (IsAligningWithPlayer()/*IsReadyToShoot()*/)
            stateMachine.ChangeState(bossNexus.bossNexus_attackState);
    }

    private bool IsAligningWithPlayer()
    {
        if (GetShootingDistance() < bossNexus.attackAcceptableRange)
            return true;

        return false;
    }

    private float GetShootingDistance()
    {
        return Vector3.Distance(bossNexus.GetMoveToPosition(), bossNexus.transform.position);
    }
}
