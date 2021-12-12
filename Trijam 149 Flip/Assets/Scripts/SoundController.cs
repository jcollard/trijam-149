using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;
    public AudioSource Bones;
    public AudioSource Coin;
    public AudioSource Death;

    public void Start()
    {
        Instance = this;
    }

    public void SetVolume(float volume)
    {
        Bones.volume = volume;
        Coin.volume = volume;
        Death.volume = volume;
    }
}
