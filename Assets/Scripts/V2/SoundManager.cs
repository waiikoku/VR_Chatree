using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource bgmPlayer;
    [SerializeField] private AudioSource sfxPlayer;

    [SerializeField] private AudioClip ringBell;
    [SerializeField] private AudioClip purchaseSFX;
    [SerializeField] private AudioClip cardDenial;

    public void PlaySFX(AudioClip clip)
    {
        sfxPlayer.PlayOneShot(clip);
    }

    public void Purchase()
    {
        PlaySFX(purchaseSFX);
    }
}
