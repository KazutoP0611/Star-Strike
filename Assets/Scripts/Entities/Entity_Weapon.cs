using UnityEngine;

public class Entity_Weapon : MonoBehaviour
{
    [Header("Bullet Details")]
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected AudioClip shootingSound;

    [Header("Shooting Point Details")]
    [SerializeField] protected Transform shootPoint;

    protected virtual void Shoot(Transform shootPoint)
    {
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        AudioSource.PlayClipAtPoint(shootingSound, shootPoint.position);
    }
}
