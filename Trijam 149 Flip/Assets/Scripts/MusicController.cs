using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    public AudioSource[] Tracks;
    public int CurrentTrack = -1;

    public float MaxVolume = 0.5f;
    public float FadeDuration = 3;
    public float FadeInStartAt = -1;
    public float FadeOutStartAt = -1;
    public float Volume;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (FadeInStartAt > 0)
        {
            HandleFadeIn();
        }
        else if (FadeOutStartAt > 0)
        {
            HandleFadeOut();
        }
    }

    public void SetTrack(int i)
    {
        int ix = i % Tracks.Length;
        if (ix == CurrentTrack)
        {
            return;
        }
        CurrentTrack = ix;
        for (int j = 0; j < Tracks.Length; j++)
        {
            if (j == ix)
            {
                Tracks[j].Play();
            }
            else
            {
                Tracks[j].Stop();
            }
        }
    }

    private void HandleFadeIn()
    {
        float EndAt = FadeInStartAt + FadeDuration;
        if (EndAt > Time.time)
        {
            float percent = (Time.time - FadeInStartAt) / FadeDuration;
            SetVolume(percent * MaxVolume);
        }
        else
        {
            SetVolume(MaxVolume);
        }
    }

    private void HandleFadeOut()
    {
        float EndAt = FadeOutStartAt + FadeDuration;
        if (EndAt > Time.time)
        {
            float percent = (Time.time - FadeOutStartAt) / FadeDuration;
            SetVolume(MaxVolume - (percent * MaxVolume));
        }
        else
        {
            SetVolume(0);
        }
    }
    public void FadeIn()
    {
        if (FadeInStartAt < 0)
        {
            FadeInStartAt = Time.time;
            FadeOutStartAt = -1;
        }
    }

    public void FadeOut()
    {
        FadeOutStartAt = Time.time;
        FadeInStartAt = -1;
    }

    public void SetVolume(float volume)
    {
        this.Volume = volume;
        foreach (AudioSource a in Tracks)
        {
            a.volume = volume;
        }
    }

}
