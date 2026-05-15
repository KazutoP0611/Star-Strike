using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 cameraParentOriginalLocation;
    private Coroutine cameraShakeCoroutine;

    [SerializeField] private Animator anim;
    [SerializeField] private string boostParameter;
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

    public static CameraController instance;

    public void CameraToBoostPosition() => anim.SetBool(boostParameter, true);

    public void CameraToNormalPosition() => anim.SetBool(boostParameter, false);

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

        Debug.LogWarning("Shake!!");
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
}
