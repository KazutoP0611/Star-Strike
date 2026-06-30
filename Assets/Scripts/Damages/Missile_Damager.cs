using UnityEngine;

public class Missile_Damager : Entity_Damager
{
    private Vector3 targetPosition;

    public void ShootAt(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    protected override void Move()
    {
        if (transform.position.z > targetPosition.z)
            transform.LookAt(targetPosition);
        else
            Destroy(gameObject);

        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
    }
}
