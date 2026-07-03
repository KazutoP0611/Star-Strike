using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Weapon : Entity_Weapon
{
    private PlayerAiming m_playerAiming;
    private bool m_isFiring;
    private float m_lastShootTime;

    #region Player Input
    // Get "Fire" input from InputAction
    public void OnFire(InputValue value) => m_isFiring = value.isPressed;
    #endregion

    [Header("General Details")]
    [SerializeField] private LayerMask shootableLayer;
    [SerializeField] private float shootInterval;
    [SerializeField] private float distanceThreshold = 1.0f;
    [SerializeField] private bool singleShoot = true;

    [Header("Shooting Transform Details")]
    [SerializeField] private Transform[] shootPoints;

    private void Awake()
    {
        m_playerAiming = GetComponent<PlayerAiming>();
    }

    private void Update()
    {
        if (!m_isFiring)
            return;

        if (Time.time < m_lastShootTime + shootInterval)
            return;

        if (singleShoot)
        {
            // Shoot bullet one at a time;
            Shoot(shootPoint);
        }
        else
        {
            // Shoot bullets from multiple points;
            foreach (var t in shootPoints)
            {
                Shoot(t);
            }
        }
    }

    public void ForceShutdownFiring() => m_isFiring = false;

    protected override void Shoot(Transform shootPoint)
    {
        Vector3 aimPoint = m_playerAiming.GetCrossHairAimingPosition();
        Ray ray = Camera.main.ScreenPointToRay(aimPoint);
        Vector3 aimVector = shootPoint.forward;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f, shootableLayer))
        {
            if (hit.collider.gameObject.transform.position.z >= transform.position.z + distanceThreshold)
            {
                aimVector = hit.collider.gameObject.transform.position - shootPoint.position;
            }
        }
        Quaternion shootDirection = Quaternion.LookRotation(aimVector);

        //Debug.DrawRay(shootPoint.position, aimVector, Color.yellow, 10.0f);

        Instantiate(bulletPrefab, shootPoint.position, shootDirection);
        AudioSource.PlayClipAtPoint(shootingSound, shootPoint.position);

        m_lastShootTime = Time.time;
    }
}
