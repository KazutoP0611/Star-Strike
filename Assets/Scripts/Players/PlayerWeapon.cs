using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private Vector2 mouseDelta;

    [SerializeField] private ParticleSystem[] laserParticles;

    [Header("Object Details")]
    [SerializeField] private Transform aimingPointTransform;
    [SerializeField] private float aimpointMovementScale = 1.5f;
    [SerializeField] private float aimpointMovingSpeed = 30f;

    [Header("Crosshair Movement Details")]
    [SerializeField] private float crosshairMovementScale = 0.8f;
    [SerializeField] private float crosshairSpeed = 30f;
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;

    [Header("Double Crosshair Details")]
    [SerializeField] private float inCrosshairDistance;
    [SerializeField] private RectTransform inCrosshairRectTransform;
    [SerializeField] private float outCrosshairDistance;
    [SerializeField] private RectTransform outCrosshairRectTransform;

    [Header("Aim Details")]
    [SerializeField] private RectTransform crosshairRectTransform;
    [SerializeField] private float aimingPointDamping = 10.0f;
    [SerializeField] private float aiminDistance;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        //CrosshairMovementHandler();
        DoubleCrosshairTransformHandler();
        AimingTransformHandler();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawRay(transform.position, transform.forward);
    //}

    private void CrosshairMovementHandler()
    {
        Vector2 moveVector = crosshairRectTransform.position + (Vector3)mouseDelta * crosshairMovementScale;
        crosshairRectTransform.position = Vector3.Lerp(crosshairRectTransform.position, moveVector, Time.deltaTime * crosshairSpeed);
    }

    private void DoubleCrosshairTransformHandler()
    {
        Vector3 inCrosshairPosition = transform.position + transform.forward * inCrosshairDistance;
        inCrosshairRectTransform.position = Camera.main.WorldToScreenPoint(inCrosshairPosition);

        Vector3 outCrosshairPosition = transform.position + transform.forward * outCrosshairDistance;
        outCrosshairRectTransform.position = Camera.main.WorldToScreenPoint(outCrosshairPosition);
    }

    private void AimingTransformHandler()
    {
        Vector3 moveVector = aimingPointTransform.position + ((Vector3)mouseDelta * aimpointMovementScale);
        moveVector.x = Mathf.Clamp(moveVector.x, horizontalLimit.x, horizontalLimit.y);
        moveVector.y = Mathf.Clamp(moveVector.y, verticalLimit.x, verticalLimit.y);
        aimingPointTransform.position = Vector3.Lerp(aimingPointTransform.position, moveVector, Time.deltaTime * aimpointMovingSpeed);
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
    public void OnMouseMove(InputValue value) => mouseDelta = value.Get<Vector2>();

    // Get "Fire" input from InputAction
    public void OnFire(InputValue value) => FiringHandler(value.isPressed);
    #endregion
}
