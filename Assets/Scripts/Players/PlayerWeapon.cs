using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private Vector2 mouseDelta;

    [Header("General Details")]
    [SerializeField] private ParticleSystem[] laserParticles;

    [Header("Aiming Point Details")]
    [SerializeField] private Transform aimingPointTransform;
    [SerializeField] private float aimpointMovementScale = 0.008f;
    [SerializeField] private float aimpointMovingSpeed = 3.0f;

    [Header("Crosshair Movement Details")]
    [SerializeField] private bool moveCrosshair = true;
    [Space]
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;
    [Space]
    [SerializeField] private Vector2 sideLimit;
    [SerializeField] private Vector2 upLimit;

    [Header("Crosshair Details")]
    [SerializeField] private bool useSingleCrosshair = false;
    [SerializeField] private RectTransform crosshairRectTransform;
    [Space]
    [SerializeField] private float inCrosshairDistance;
    [SerializeField] private RectTransform inCrosshairRectTransform;
    [SerializeField] private float outCrosshairDistance;
    [SerializeField] private RectTransform outCrosshairRectTransform;

    #region Player Input
    public void OnMouseMove(InputValue value) => mouseDelta = value.Get<Vector2>();

    // Get "Fire" input from InputAction
    public void OnFire(InputValue value) => FiringHandler(value.isPressed);
    #endregion

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

    // Update single crosshair => This one is simple, just align crosshair to aiming object's transform;
    private void CrosshairMovementHandler()
    {
        crosshairRectTransform.position = Camera.main.WorldToScreenPoint(aimingPointTransform.position);
    }

    // Update double crosshair's transform;
    private void DoubleCrosshairTransformHandler()
    {
        // Inside crosshair movements
        Vector3 inCrosshairPosition = transform.position + transform.forward * inCrosshairDistance;
        Vector3 inCrosshairWorldPosition = transform.parent.TransformPoint(inCrosshairPosition);
        inCrosshairRectTransform.position = Camera.main.WorldToScreenPoint(inCrosshairPosition);

        // Outside crosshair movements
        Vector3 outCrosshairPosition = transform.position + transform.forward * outCrosshairDistance;
        Vector3 outCrosshairWorldPosition = transform.parent.TransformPoint(outCrosshairPosition);
        outCrosshairRectTransform.position = Camera.main.WorldToScreenPoint(outCrosshairPosition);
    }

    // Update aiming object transform => Player's movement will follow this object
    private void AimingTransformHandler()
    {
        Transform tempTransform = aimingPointTransform;

        // Calculate target position;
        Vector3 moveToPoint = tempTransform.localPosition + new Vector3(mouseDelta.x, mouseDelta.y, 0) * aimpointMovementScale;

        // Limit aiming transform inside imagine rectangle, or aiming will keep moving without limit;
        float sidePosition = Mathf.Clamp(moveToPoint.x, sideLimit.x, sideLimit.y);
        float upPosition = Mathf.Clamp(moveToPoint.y, upLimit.x, upLimit.y);
        Vector3 moveToLocalPosition = new Vector3(sidePosition, upPosition, aimingPointTransform.localPosition.z);

        // Set aiming's transform to calculated limited target position;
        aimingPointTransform.localPosition = moveToLocalPosition;
    }

    // Enable firing (laser) particle
    private void FiringHandler(bool fire)
    {
        foreach (var particle in laserParticles)
        {
            var laserEmission = particle.emission;
            laserEmission.enabled = fire;
        }
    }
}
