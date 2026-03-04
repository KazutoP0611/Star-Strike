using UnityEngine;

public class PlayerFlightControl : MonoBehaviour
{
    [SerializeField] private Transform aimingObject;
    [SerializeField] private float rotateSpeed = 30.0f;

    private void Update()
    {
        Vector3 lookatDirection = aimingObject.position - transform.position;

        Quaternion lookatQuaternion = Quaternion.LookRotation(lookatDirection, Vector3.up);
        Quaternion rotateValue = Quaternion.Lerp(transform.rotation, lookatQuaternion, Time.deltaTime * rotateSpeed);
        transform.rotation = rotateValue;
    }
}
