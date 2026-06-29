using UnityEngine;
using UnityEngine.Playables;

public class BossSequenceManager : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] private GameObject boss;

    [Header("Playable Director Details")]
    [SerializeField] private PlayableDirector bossAppearanceTimeline;
    [SerializeField] private PlayableDirector bossDeadTimeline;

    [ContextMenu("Play")]
    public void PlayBossAppear()
    {
        bossAppearanceTimeline.Play();
    }

    public void OnBossAppearStart()
    {
        UI_Manager.instance.SetActiveUIIndicators(false);
        Player.Instance.EnablePlayerInput(false);
    }

    public void OnBossAppearEnd()
    {
        UI_Manager.instance.SetActiveUIIndicators(true);
        Player.Instance.EnablePlayerInput(true);
    }

    public void SetActiveBoss(bool active) => boss.SetActive(active);
}
