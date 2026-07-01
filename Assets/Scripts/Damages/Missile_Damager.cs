using UnityEngine;

public class Missile_Damager : Entity_Damager
{
    private GameObject m_playerGO;
    private Vector3 m_currentAimingTargetPosition;

    [SerializeField] private float aimingRotationSpeed = 2.2f;
    //[SerializeField] private float destroyPositionThreshold = 2.0f;

    public void ShootAt(GameObject playerGO)
    {
        m_playerGO = playerGO;
    }

    protected override void Update()
    {
        if (move == false)
            return;

        CheckPosition();
        Turning();
        Move();
    }

    private void CheckPosition()
    {
        if (transform.position.z < m_playerGO.transform.position.z/* - destroyPositionThreshold*/)
            Destroy(gameObject);
    }

    private void Turning()
    {
        Vector3 vectorToPlayer = (m_playerGO.transform.position - transform.position).normalized;
        m_currentAimingTargetPosition = Vector3.Lerp(transform.forward, vectorToPlayer, Time.deltaTime * aimingRotationSpeed);

        transform.forward = m_currentAimingTargetPosition;
    }
}
