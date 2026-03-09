using UnityEngine;

public class Enemy_Health : Entity_Health
{
    protected override void OnDead()
    {
        base.OnDead();

        Destroy(gameObject);
    }
}
