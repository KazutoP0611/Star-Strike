using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private Vector2 mouseDelta;

    [Header("General Details")]
    [SerializeField] private ParticleSystem[] laserParticles;

    [Header("Aiming Point Details")]
    [SerializeField] private Transform aimingPointTransform;
    [SerializeField] private float aimpointMovementScale = 1.5f;
    [SerializeField] private float aimpointMovingSpeed = 30f;
    [Space]
    [SerializeField] private bool useSingleCrosshair = false;

    [Header("Crosshair Movement Details")]
    [SerializeField] private float crosshairMovementScale = 0.8f;
    [SerializeField] private float crosshairSpeed = 30f;
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;

    [Header("Crosshair Setting Details")]
    [SerializeField] private RectTransform crosshairRectTransform;

    [Header("Double Crosshair Details")]
    [SerializeField] private float inCrosshairDistance;
    [SerializeField] private RectTransform inCrosshairRectTransform;
    [SerializeField] private float outCrosshairDistance;
    [SerializeField] private RectTransform outCrosshairRectTransform;

    private void Start()
    {
        Cursor.visible = false;

        crosshairRectTransform.gameObject.SetActive(useSingleCrosshair);
        inCrosshairRectTransform.gameObject.SetActive(!useSingleCrosshair);
        outCrosshairRectTransform.gameObject.SetActive(!useSingleCrosshair);
    }

    private void Update()
    {
        if (useSingleCrosshair)
            CrosshairMovementHandler();
        else
            DoubleCrosshairTransformHandler();

        AimingTransformHandler();
    }

    private void CrosshairMovementHandler()
    {
        crosshairRectTransform.position = Camera.main.WorldToScreenPoint(aimingPointTransform.position);
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
        Vector3 moveVector = aimingPointTransform.position + new Vector3(mouseDelta.x, mouseDelta.y, 0) * aimpointMovementScale;

        // Just like player's movement. Use clamp calculation as below, will not require developers to input new limit position everytime plane's position changes;
        //moveVector.x = Mathf.Clamp(moveVector.x, horizontalLimit.x, horizontalLimit.y);
        //moveVector.y = Mathf.Clamp(moveVector.y, verticalLimit.x, verticalLimit.y);

        // New Clamp Calculation;
        Vector3 aimingPointNewPosition = Camera.main.WorldToViewportPoint(moveVector);
        aimingPointNewPosition.x = Mathf.Clamp(aimingPointNewPosition.x, horizontalLimit.x, horizontalLimit.y);
        aimingPointNewPosition.y = Mathf.Clamp(aimingPointNewPosition.y, verticalLimit.x, verticalLimit.y);

        Vector3 aimingPointWorldPosition = Camera.main.ViewportToWorldPoint(aimingPointNewPosition);
        aimingPointWorldPosition.z = aimingPointTransform.position.z;

        // Assign position instead of damping value
        // Because plane already has limit position and damping, using more damping will give weird feeling when aimingObject is near edge of the screen;
        aimingPointTransform.position = aimingPointWorldPosition;
        //aimingPointTransform.position = Vector3.Lerp(aimingPointTransform.position, aimingPointWorldPosition, Time.deltaTime * aimpointMovingSpeed);
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
