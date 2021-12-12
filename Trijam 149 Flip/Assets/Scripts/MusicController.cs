using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    public AudioSource[] Tracks;
    public float[] Volumes;
    public int CurrentTrack = -1;

    public float MaxVolume = 0.5f;
    public float FadeDuration = 3;
    public float[] FadeInStartAt;
    public float[] FadeOutStartAt;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Volumes = new float[Tracks.Length];
        FadeInStartAt = new float[Tracks.Length];
        FadeOutStartAt = new float[Tracks.Length];

        for (int i = 0; i < Volumes.Length; i++)
        {
            Volumes[i] = 0;
            FadeInStartAt[i] = -1;
            FadeOutStartAt[i] = -1;
        }
    }

    void Update()
    {
        HandleFadeIn();
        HandleFadeOut();

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
                FadeInStartAt[j] = Time.time;
                FadeOutStartAt[j] = -1;
            }
            else
            {
                FadeOutStartAt[j] = Time.time;
                FadeInStartAt[j] = -1;
            }
        }
    }

    private void HandleFadeIn()
    {
        for (int i = 0; i < Tracks.Length; i++)
        {
            if (i == CurrentTrack && !Tracks[i].isPlaying)
            {
                Tracks[i].Play();
            }
            float EndAt = FadeInStartAt[i] + FadeDuration;
            if (EndAt > Time.time)
            {
                float percent = (Time.time - FadeInStartAt[i]) / FadeDuration;
                SetVolume(percent * MaxVolume, i);
            }
            else
            {
                SetVolume(MaxVolume, i);
            }
        }
    }

    private void HandleFadeOut()
    {
        for (int i = 0; i < Tracks.Length; i++)
        {
            if (i == CurrentTrack)
            {
                continue;
            }
            float EndAt = FadeOutStartAt[i] + FadeDuration;
            if (EndAt > Time.time)
            {
                float percent = (Time.time - FadeOutStartAt[i]) / FadeDuration;
                SetVolume(MaxVolume - (percent * MaxVolume), i);
            }
            else
            {
                SetVolume(0, i);
                Tracks[i].Stop();
            }
        }
    }
    public void SetVolume(float volume, int track)
    {
        this.Volumes[track] = volume;
        this.Tracks[track].volume = volume;
    }

}
