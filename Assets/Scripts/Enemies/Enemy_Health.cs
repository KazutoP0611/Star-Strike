using UnityEngine;

public class Enemy_Health : Entity_Health
{
    [SerializeField] private GameObject model;

    public override void TakeDamage(Collider hitObject)
    {
        base.TakeDamage(hitObject);

        //maybe add hurt state?
        //play some animations? something like that.
    }

    protected override void Die()
    {
        base.Die();

        Destroy(gameObject);
        //model.SetActive(false);

        CameraController.instance.CameraShake();
    }
}
