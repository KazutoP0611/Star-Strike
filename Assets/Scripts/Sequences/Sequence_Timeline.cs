using UnityEngine;
using UnityEngine.Playables;

public class Sequence_Timeline : Entity_Sequence
{
    [SerializeField] private PlayableDirector playableTimeline;

    public override void SequenceTrigger()
    {
        base.SequenceTrigger();

        playableTimeline.Play();
    }
}
