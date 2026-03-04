using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -10);
    public float followSmoothness = 5f;
    public float rotationSmoothness = 5f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.TransformPoint(offset);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSmoothness * Time.deltaTime
        );

        Quaternion desiredRotation = Quaternion.LookRotation(target.forward);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            desiredRotation,
            rotationSmoothness * Time.deltaTime
        );
    }
}