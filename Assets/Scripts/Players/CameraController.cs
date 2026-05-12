using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string boostParameter;

    [Header("Player Follower")]
    [SerializeField] private bool followPlayer = true;
    [SerializeField] private GameObject player;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private Vector3 moveToOffset;

    public void CameraToBoostPosition() => anim.SetBool(boostParameter, true);

    public void CameraToNormalPosition() => anim.SetBool(boostParameter, false);

    private void Update()
    {
        if (followPlayer)
        {
            Vector3 moveToPosition = player.transform.localPosition + moveToOffset;
            moveToPosition.z = transform.localPosition.z;
            
            //TODO
            // - Limit camera movement, not follow too perfect or fighter will always be in the middle;

            transform.localPosition = Vector3.Lerp(transform.localPosition, moveToPosition, Time.deltaTime * moveSpeed);
        }
    }
}
