using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_SoundDamagochi : SH_PoolDamagochi
{
    AudioSource speaker;
    protected override void Awake()
    {
        base.Awake();
        if(!TryGetComponent<AudioSource>(out speaker))
        {
            speaker = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip clip, bool playOneShot = true)
    {
        if (playOneShot)
            speaker.PlayOneShot(clip);
        else
        {
            speaker.clip = clip;
            speaker.Play();
        }
    }
}
