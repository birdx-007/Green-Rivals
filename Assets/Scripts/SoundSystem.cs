using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static bool shouldPlaySpraySound = false;
    public AudioClip spraySound;
    public static bool shouldPlayCollectSound = false;
    public AudioClip collectSound;
    private static AudioSource soundSource;
    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }
    public void InitiateOnRoundStart()
    {
        shouldPlaySpraySound = false;
        shouldPlayCollectSound = false;
    }
    public async void PlayPlayerSound()
    {
        await UniTask.Delay(800); // 对齐动画延时
        if (shouldPlaySpraySound)
        {
            soundSource.PlayOneShot(spraySound);
        }
        if (shouldPlayCollectSound)
        {
            soundSource.PlayOneShot(collectSound);
        }
    }
}
