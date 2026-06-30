using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Weapon : Entity_Weapon
{
    private bool isFiring;
    private float nextShootTime;

    #region Player Input
    // Get "Fire" input from InputAction
    public void OnFire(InputValue value) => isFiring = value.isPressed;
    #endregion

    [Header("General Details")]
    [SerializeField] private bool singleShoot = true;
    [SerializeField] private float shootInterval;

    [Header("Shooting Transform Details")]
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

    public void ForceShutdownFiring() => isFiring = false;
}
