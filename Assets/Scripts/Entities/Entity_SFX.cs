using UnityEngine;

public enum SoundType
{
    Damage,
    Destroyed
}

public class Entity_SFX : MonoBehaviour
{
    [Header("Sound Details")]
    [SerializeField] private AudioClip onDamageSound;
    [SerializeField] private AudioClip onDestroyedSound;

    public void PlaySoundAtPoint(SoundType soundType)
    {
        switch(soundType)
        {
            case SoundType.Damage:
                AudioSource.PlayClipAtPoint(onDamageSound, transform.position);
                break;
            case SoundType.Destroyed:
                AudioSource.PlayClipAtPoint(onDestroyedSound, transform.position);
                break;
        }
    }
}
