using UnityEngine;
using UnityEngine.Playables;

public class BossSequenceManager : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] private GameObject boss;

    [Header("Playable Director Details")]
    [SerializeField] private PlayableDirector bossAppearanceTimeline;
    [SerializeField] private PlayableDirector bossDeadTimeline;

    [ContextMenu("Start Boss")]
    public void StartSequence()
    {
        SetActiveBoss(true);
        BossNexus bossNexus = boss.GetComponent<BossNexus>();
        bossNexus.Initialize(StartBossDieSequence);

        bossAppearanceTimeline.Play();
    }

    public void OnSequenceStart()
    {
        UI_Manager.instance.SetActiveUIIndicators(false);
        Player.Instance.EnablePlayerInput(false);
    }

    public void OnSequenceEnd()
    {
        UI_Manager.instance.SetActiveUIIndicators(true);
        Player.Instance.EnablePlayerInput(true);
    }

    public void StartBossDieSequence()
    {
        bossDeadTimeline.Play();
    }

    public void SetActiveBoss(bool active) => boss.SetActive(active);
}
