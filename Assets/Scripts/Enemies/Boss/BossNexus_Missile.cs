using UnityEngine;

public class BossNexus_Missile : MonoBehaviour
{
    private GameObject m_playerGO;
    private float m_timeCount;
    private bool m_isActivated = false;

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private float shootMissleDotThreshold = 0.9f;
    [SerializeField] private float shootDelay = 2.0f;

    public void Initialize(GameObject playerGO)
    {
        m_playerGO = playerGO;

        m_timeCount = shootDelay;
    }

    private void Update()
    {
        if (m_isActivated == false)
            return;

        if (m_timeCount >= 0)
        {
            m_timeCount -= Time.deltaTime;
            return;
        }

        CheckShootMissile();
    }

    private void CheckShootMissile()
    {
        Vector3 vectorToPlayer = m_playerGO.transform.position - transform.position;
        float dotValue = Vector3.Dot(transform.forward, vectorToPlayer.normalized);

        if (dotValue > shootMissleDotThreshold)
        {
            Missile_Damager missile = Instantiate(
                    missilePrefab,
                    transform.position + transform.forward * 0.5f,
                    transform.rotation
                ).GetComponent<Missile_Damager>();

            missile.ShootAt(m_playerGO);

            //m_lastShootTime = Time.time;
            m_timeCount = shootDelay;
        }
    }

    public void SetActivateMissile(bool activate) => m_isActivated = activate;
}
