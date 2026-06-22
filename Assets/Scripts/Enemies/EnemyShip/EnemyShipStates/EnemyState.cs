using UnityEngine;

public class EnemyState : EntityState
{
    protected EnemyShip enemy;

    public EnemyState(EnemyShip enemy, StateMachine stateMachine) : base(stateMachine)
    {
        this.enemy = enemy;
    }
}
