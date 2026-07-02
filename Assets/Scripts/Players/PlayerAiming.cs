using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    private Vector2 mouseDelta;

    #region Player Input
    public void OnMouseMove(InputValue value) => mouseDelta = value.Get<Vector2>();
    #endregion

    [Header("Aiming Point Details")]
    [SerializeField] private Transform aimingPointTransform;
    [SerializeField] private float aimpointMovementScale = 0.008f;

    [Header("Crosshair Movement Details")]
    [SerializeField] private bool moveCrosshair = true;
    [Space]
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;

    [Header("Crosshair Details")]
    [SerializeField] private bool useSingleCrosshair = false;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private RectTransform crosshairRectTransform;
    [Space]
    [SerializeField] private float inCrosshairDistance;
    [SerializeField] private RectTransform inCrosshairRectTransform;
    [SerializeField] private float outCrosshairDistance;
    [SerializeField] private RectTransform outCrosshairRectTransform;

    private void Start()
    {
        UI_Manager.instance.SetActiveCursor(false);

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
            SingleCrosshairMovementHandler();
        else
            DoubleCrosshairTransformHandler();
        //------------------------------------

        // crossPos = GetCrossHairAimingPosition();
        //Debug.DrawRay(crossPos, Camera.main.transform.forward, Color.yellow);
    }

    private void OnDrawGizmos()
    {
        Vector3 crossPos = GetCrossHairAimingPosition();

        Gizmos.DrawWireSphere(crossPos + Camera.main.transform.forward, .05f);
        //Gizmos.DrawRay(crossPos, Camera.main.transform.forward);
    }

    // Update single crosshair => This one is simple, just align crosshair to aiming object's transform;
    private void SingleCrosshairMovementHandler()
    {
        crosshairRectTransform.position = Camera.main.WorldToScreenPoint(aimingPointTransform.position);
    }

    // Update double crosshair's transform;
    private void DoubleCrosshairTransformHandler()
    {
        // Inside crosshair movements
        Vector3 inCrosshairPosition = shootingPoint.position + shootingPoint.forward * inCrosshairDistance;
        Vector3 inCrosshairWorldPosition = shootingPoint.parent.TransformPoint(inCrosshairPosition);
        inCrosshairRectTransform.position = Camera.main.WorldToScreenPoint(inCrosshairPosition);

        // Outside crosshair movements
        Vector3 outCrosshairPosition = shootingPoint.position + shootingPoint.forward * outCrosshairDistance;
        Vector3 outCrosshairWorldPosition = shootingPoint.parent.TransformPoint(outCrosshairPosition);
        outCrosshairRectTransform.position = Camera.main.WorldToScreenPoint(outCrosshairPosition);
    }

    // Update aiming object transform => Player's movement will follow this object
    private void AimingTransformHandler()
    {
        Transform tempTransform = aimingPointTransform;

        // Calculate target position;
        Vector3 moveToPoint = tempTransform.localPosition + new Vector3(mouseDelta.x, mouseDelta.y, 0) * aimpointMovementScale;

        // Limit aiming transform inside imagine rectangle, or aiming will keep moving without limit;
        float horizontalPosition = Mathf.Clamp(moveToPoint.x, horizontalLimit.x, horizontalLimit.y);
        float verticalPosition = Mathf.Clamp(moveToPoint.y, verticalLimit.x, verticalLimit.y);
        Vector3 moveToLocalPosition = new Vector3(horizontalPosition, verticalPosition, aimingPointTransform.localPosition.z);

        // Set aiming's transform to calculated limited target position;
        aimingPointTransform.localPosition = moveToLocalPosition;
    }

    public Vector3 GetCrossHairAimingPosition()
    {
        RectTransform currentCrossHair = useSingleCrosshair ? crosshairRectTransform : inCrosshairRectTransform;
        return currentCrossHair.position;
    }
}
