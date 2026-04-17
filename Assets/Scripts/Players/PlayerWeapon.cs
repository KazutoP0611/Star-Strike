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
    [SerializeField] private bool moveCrosshair = true;
    [Space]
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
        AimingTransformHandler();

        // Crosshair Moving
        if (!moveCrosshair)
            return;

        if (useSingleCrosshair)
            CrosshairMovementHandler();
        else
            DoubleCrosshairTransformHandler();
        //------------------------------------
    }

    private void CrosshairMovementHandler()
    {
        crosshairRectTransform.localPosition = Camera.main.WorldToScreenPoint(aimingPointTransform.localPosition);
    }

    private void DoubleCrosshairTransformHandler()
    {
        Vector3 inCrosshairPosition = transform.localPosition + transform.forward * inCrosshairDistance;
        inCrosshairRectTransform.localPosition = Camera.main.WorldToScreenPoint(inCrosshairPosition);

        Vector3 outCrosshairPosition = transform.localPosition + transform.forward * outCrosshairDistance;
        outCrosshairRectTransform.localPosition = Camera.main.WorldToScreenPoint(outCrosshairPosition);
    }

    private void AimingTransformHandler()
    {
        Transform tempTransform = aimingPointTransform;

        Vector3 moveToPoint = tempTransform.localPosition + new Vector3(mouseDelta.x, mouseDelta.y, 0) * aimpointMovementScale;
        tempTransform.localPosition = moveToPoint;

        Vector3 aimingPointNewPosition = Camera.main.WorldToViewportPoint(tempTransform.position);
        aimingPointNewPosition.x = Mathf.Clamp(aimingPointNewPosition.x, horizontalLimit.x, horizontalLimit.y);
        aimingPointNewPosition.y = Mathf.Clamp(aimingPointNewPosition.y, verticalLimit.x, verticalLimit.y);

        Vector3 aimingPointWorldPosition = Camera.main.ViewportToWorldPoint(aimingPointNewPosition);

        Vector3 localPosition = transform.parent.InverseTransformPoint(aimingPointWorldPosition);
        localPosition.z = aimingPointTransform.localPosition.z;

        aimingPointTransform.localPosition = localPosition;
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
