using UnityEngine;

public class Enemy_Weapon : Entity_Weapon
{
    //public GameObject bullet { get; private set; }

    public void Shoot()
    {
        Shoot(shootPoint);
    }

    protected override void Shoot(Transform shootPoint)
    {
        //float bulletRotation = Random.Range(0, 2) == 0 ? 0 : 90;

        float bulletRotation = Random.Range(0, 181);
        bulletRotation = Mathf.Ceil(bulletRotation);

        Vector3 rotation = shootPoint.rotation.eulerAngles;
        rotation.z = bulletRotation;

        /*bullet = */Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(rotation));
        AudioSource.PlayClipAtPoint(shootingSound, shootPoint.position);
    }
}
