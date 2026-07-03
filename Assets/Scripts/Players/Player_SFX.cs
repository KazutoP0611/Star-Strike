using System.Security.Cryptography;
using UnityEngine;

public enum ChangeSpeedSound
{
    Boost,
    Break
}

public class Player_SFX : Entity_SFX
{
    [Header("Speed Sound Details")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip boostClip;
    [SerializeField] private AudioClip breakClip;

    public void PlaySpeedChangeSound(ChangeSpeedSound changeSpeedSound)
    {
        switch (changeSpeedSound)
        {
            case ChangeSpeedSound.Boost:
                audioSource.clip = boostClip;
                break;
            case ChangeSpeedSound.Break:
                audioSource.clip = breakClip;
                break;
        }

        audioSource.time = 0f;
        audioSource.Play();
    }
}
