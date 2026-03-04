using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private Vector2 mousePosition;

    [SerializeField] private ParticleSystem[] laserParticles;

    [Header("Aim Details")]
    [SerializeField] private RectTransform crosshairRectTransform;
    [SerializeField] private Transform aimingPointTransform;
    [SerializeField] private float aimingPointDamping = 10.0f;
    [SerializeField] private float aiminDistance;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        CrosshairMovementHandler();
        AimingPointHandler();
    }

    private void CrosshairMovementHandler()
    {
        crosshairRectTransform.position = mousePosition;
    }

    private void AimingPointHandler()
    {
        Vector3 aimingPosition = new Vector3(mousePosition.x, mousePosition.y, aiminDistance);
        aimingPointTransform.position = Vector3.Lerp(aimingPointTransform.position, Camera.main.ScreenToWorldPoint(aimingPosition), Time.deltaTime * aimingPointDamping);
    }

    private void FiringHandler(bool fire)
    {
        foreach (var particle in laserParticles)
        {
            var laserEmission = particle.emission;
            laserEmission.enabled = fire;
        }
    }

    #region Player Input
    public void OnMouseMove(InputValue value) => mousePosition = value.Get<Vector2>();

    // Get "Fire" input from InputAction
    public void OnFire(InputValue value) => FiringHandler(value.isPressed);
    #endregion
}
