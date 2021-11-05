using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_SoundManager : MonoBehaviour
{
    public AudioSource bgmPlayer;
    public AudioSource effectPlayer;

    public AudioClip[] bgmList;

    public void PlayBattleBGM()
    {
        var bgm = bgmList[Random.Range(0, bgmList.Length)];
        bgmPlayer.clip = bgm;
        bgmPlayer.loop = true;
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        StartCoroutine(VolumSlowDown());
    }

    IEnumerator VolumSlowDown()
    {
        while (bgmPlayer.volume > 0)
        {
            bgmPlayer.volume -= Time.deltaTime;
            yield return null;
        }
    }
}
