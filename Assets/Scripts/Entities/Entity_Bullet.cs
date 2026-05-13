using UnityEngine;

public class Entity_Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 30.0f;

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
    }
}
