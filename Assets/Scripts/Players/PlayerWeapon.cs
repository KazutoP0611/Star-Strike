using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private bool singleShoot = true;
    private bool isFiring;
    private float nextShootTime;

    #region Player Input
    // Get "Fire" input from InputAction
    public void OnFire(InputValue value) => FiringHandler(value.isPressed);
    #endregion

    [Header("General Details")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float shootInterval;

    [Header("Shooting Transform Details")]
    [SerializeField] private Transform shootPoint;
    [Space]
    [SerializeField] private Transform[] shootPoints;

    private void Update()
    {
        if (!isFiring)
            return;

        if (Time.time < nextShootTime)
            return;

        if (singleShoot)
        {
            // Shoot bullet one at a time;
            Shoot(shootPoint);
        }
        else
        {
            // Shoot multiple bullets
            foreach (var t in shootPoints)
            {
                Shoot(t);
            }
        }

        // Set next able to shoot timing;
        CalculateShootInterval();
    }

    private void CalculateShootInterval()
    {
        nextShootTime = Time.time + shootInterval;
    }

    // Enable firing (laser) particle
    private void FiringHandler(bool fire)
    {
        isFiring = fire;
    }

    private void Shoot(Transform shootPoint)
    {
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        audioSource.Play(0);
    }
}
