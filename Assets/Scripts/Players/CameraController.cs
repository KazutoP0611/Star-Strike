using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string boostParameter;

    [Header("Player Follower Details")]
    [SerializeField] private GameObject cameraParent;
    [Space]
    [SerializeField] private bool followPlayer = true;
    [SerializeField] private GameObject player;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private Vector3 moveToOffset;
    [Space]
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;

    public void CameraToBoostPosition() => anim.SetBool(boostParameter, true);

    public void CameraToNormalPosition() => anim.SetBool(boostParameter, false);

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

    private void CameraShake()
    {

    }
}
