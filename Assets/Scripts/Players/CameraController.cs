using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private Vector3 cameraParentOriginalLocation;
    private Coroutine cameraShakeCoroutine;
    private bool isAnimBusy;

    [SerializeField] private Animator anim;
    [SerializeField] private GameObject cameraParent;

    [Header("Player Follower Details")]
    [SerializeField] private bool followPlayer = true;
    [SerializeField] private GameObject player;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private Vector3 moveToOffset;
    [Space]
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;

    [Header("Camera Shake Details")]
    [SerializeField] private float shakeDuration = 1.0f;
    [SerializeField] private float shakeMagnitude = 2.0f;
    [SerializeField] private float dampingSpeed = 1.0f;
    [Space]
    [SerializeField] private Vector2 xShakeLimit;
    [SerializeField] private Vector2 yShakeLimit;

    [Header("Animation Details")]
    [SerializeField] private string boostStringParameter;
    [SerializeField] private string normalStringParam;
    [SerializeField] private string breakStringParam;

    public void CameraToBoostPosition() => SetCameraAnimation(boostStringParameter, false);
    public void CameraToNormalPosition() => SetCameraAnimation(normalStringParam, true);
    public void CameraToBreakPosition() => SetCameraAnimation(breakStringParam, false);
    public void ResetCameraTriggerParams()
    {
        anim.ResetTrigger(boostStringParameter);
        anim.ResetTrigger(normalStringParam);
        anim.ResetTrigger(breakStringParam);
    }

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // DontDestroyOnLoad(gameObject);

        cameraParentOriginalLocation = cameraParent.transform.localPosition;
    }

    private void Update()
    {
        if (followPlayer)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 moveToPosition = player.transform.localPosition + moveToOffset;
        moveToPosition.z = transform.localPosition.z;

        // Limit camera movement, not follow too perfect or fighter will always be in the middle;
        float horizontalPosition = Mathf.Clamp(moveToPosition.x, horizontalLimit.x, horizontalLimit.y);
        float verticalPosition = Mathf.Clamp(moveToPosition.y, verticalLimit.x, verticalLimit.y);
        Vector3 moveToLocalPosition = new Vector3(horizontalPosition, verticalPosition, transform.localPosition.z);

        // Set calculated clamped position to this object;
        transform.localPosition = Vector3.Lerp(transform.localPosition, moveToLocalPosition, Time.deltaTime * moveSpeed);

        // Noted:
        // can not use cameraParent right now, need to fix this point;
    }

    public void CameraShake()
    {
        if (cameraShakeCoroutine != null)
            StopCoroutine(cameraShakeCoroutine);

        cameraShakeCoroutine = StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        float elapsedTime = 0;
        Vector3 initialPosition = cameraParent.transform.localPosition;

        while (elapsedTime < shakeDuration)
        {
            float magnitude = shakeMagnitude * Mathf.Exp(-dampingSpeed * elapsedTime);
            float xOffset = Random.Range(xShakeLimit.x, xShakeLimit.y) * magnitude;
            float yOffset = Random.Range(yShakeLimit.x, yShakeLimit.y) * magnitude;

            cameraParent.transform.localPosition = new Vector3(initialPosition.x + xOffset, initialPosition.y + yOffset, initialPosition.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        cameraParent.transform.localPosition = initialPosition;
    }

    private void SetCameraAnimation(string animParam, bool normal)
    {
        // If animParam is NOT normal, can not play anim;
        if (isAnimBusy && normal == false)
            return;

        // If method doesn't send normal, means anim is busy;
        isAnimBusy = !normal;

        anim.SetTrigger(animParam);
        Debug.LogWarning($"Anim Status : {animParam} is {isAnimBusy}");
    }
}
