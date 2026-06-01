using UnityEngine;

public class Enemy_Health : Entity_Health
{
    [SerializeField] private GameObject model;

    protected override void OnDead()
    {
        base.OnDead();

        //Destroy(gameObject);
        model.SetActive(false);

        CameraController.instance.CameraShake();
    }
}
