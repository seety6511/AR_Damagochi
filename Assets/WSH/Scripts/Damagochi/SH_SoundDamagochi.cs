using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamagochiSound
{
    Idle,
    Walk,
    Run,
    Fly,
    Eat,
    Dead,
    Skill_01,
    Skill_02,
    Skill_03,
    Skill_04,
}
public class SH_SoundDamagochi : SH_PoolDamagochi
{
    SH_SoundContainer soundContainer;
    AudioSource speaker;
    protected override void Awake()
    {
        base.Awake();
        if(!TryGetComponent<AudioSource>(out speaker))
            speaker = gameObject.AddComponent<AudioSource>();
        soundContainer = GetComponent<SH_SoundContainer>();
    }

    public void PlaySound(DamagochiSound soundLabel, bool playOneShot = true)
    {
        switch (soundLabel)
        {
            case DamagochiSound.Idle:
                PlaySound(soundContainer.idle, playOneShot);
                break;
            case DamagochiSound.Walk:
                PlaySound(soundContainer.walk, playOneShot);
                break;
            case DamagochiSound.Run:
                PlaySound(soundContainer.run, playOneShot);
                break;
            case DamagochiSound.Fly:
                PlaySound(soundContainer.fly, playOneShot);
                break;
            case DamagochiSound.Dead:
                PlaySound(soundContainer.dead, playOneShot);
                break;
            case DamagochiSound.Eat:
                PlaySound(soundContainer.eat, playOneShot);
                break;
            case DamagochiSound.Skill_01:
                PlaySound(soundContainer.skill_01, playOneShot);
                break;
            case DamagochiSound.Skill_02:
                PlaySound(soundContainer.skill_02, playOneShot);
                break;
            case DamagochiSound.Skill_03:
                PlaySound(soundContainer.skill_03, playOneShot);
                break;
            case DamagochiSound.Skill_04:
                PlaySound(soundContainer.skill_04, playOneShot);
                break;
        }
    }

    public void PlaySound(AudioClip clip, bool playOneShot = true)
    {
        if(clip == null)
        {
            Debug.Log("Sound Clip is NULL.");
            return;
        }    

        if (playOneShot)
            speaker.PlayOneShot(clip);
        else
        {
            speaker.clip = clip;
            speaker.Play();
        }
    }
}
