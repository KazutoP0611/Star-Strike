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

    [Header("Crosshair Movement Details")]
    [SerializeField] private bool moveCrosshair = true;
    [Space]
    //[SerializeField] private float crosshairMovementScale = 0.8f;
    //[SerializeField] private float crosshairSpeed = 30f;
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;

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
