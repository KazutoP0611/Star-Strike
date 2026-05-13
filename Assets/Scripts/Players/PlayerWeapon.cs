using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private bool singleShoot = true;
    private float time;

    #region Player Input
    // Get "Fire" input from InputAction
    public void OnFire(InputValue value) => FiringHandler(value.isPressed);
    #endregion

    [Header("General Details")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval;

    [Header("Shooting Transform Details")]
    [SerializeField] private Transform shootPoint;
    [Space]
    [SerializeField] private Transform[] shootPoints;

    // Enable firing (laser) particle
    private void FiringHandler(bool fire)
    {
       if (singleShoot)
       {
            Shoot(shootPoint);
       }
       else
       {

       }
    }

    private void Shoot(Transform shootPoint)
    {
        if (time > shootInterval)
        {
            time = 0;
            return;
        }

        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        time += Time.deltaTime;
    }
}
