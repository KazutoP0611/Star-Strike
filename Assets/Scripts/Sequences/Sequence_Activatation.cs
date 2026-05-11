using UnityEngine;

public class Sequence_Activatation : Entity_Sequence
{
    [SerializeField] private GameObject[] gameObjects;

    public override void SequenceTrigger()
    {
        base.SequenceTrigger();

        foreach (var obj in gameObjects)
        {
            obj.SetActive(true);
        }
    }

    public override void EndSequence()
    {
        base.EndSequence();

        foreach (var obj in gameObjects)
        {
            obj.SetActive(false);
        }
    }
}
