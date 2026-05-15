using UnityEngine;

public class Enemy_Health : Entity_Health
{
    [SerializeField] private GameObject enemyModel;

    protected override void OnDead()
    {
        base.OnDead();

        //Destroy(gameObject);
        enemyModel.SetActive(false);

        CameraController.instance.CameraShake();
    }
}
