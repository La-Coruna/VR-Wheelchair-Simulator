using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 휠체어 훈련 나레이션 플레이어
public class TrainingAudioPlayer : MonoBehaviour
{
    public AudioClip[] soundClips;
    public AudioClip GoodJobClips;
    private AudioSource audioSource;
    public int currentClipIndex = 0;
    public AudioClip endTrainingClip;
    private bool isEnded = false;
    private bool is5MinutesPassed = false;
    public float limitTime = 300;
    
    void Start()
    {
        // AudioSource 컴포넌트를 추가하고 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
        if (soundClips.Length > 0)
        {
            audioSource.clip = soundClips[currentClipIndex];
        }
    }

    private void Update()
    {
        // 단축키
        if (Input.GetKeyDown(KeyCode.Alpha1)) PlayGodJobSound();
        if (Input.GetKeyDown(KeyCode.Alpha2)) PlaySound();
        if (Input.GetKeyDown(KeyCode.Alpha3)) PlayNextSound();
        if (Input.GetKeyDown(KeyCode.P) && !isEnded) FollowProcedure();
        if (is5MinutesPassed && !audioSource.isPlaying)
        {
            PlayEndTrainingSound();
            is5MinutesPassed = false;
        }
    }

    public void PlaySound()
    {
        if (soundClips.Length > 0)
        {
            audioSource.clip = soundClips[currentClipIndex];
            audioSource.Play();
        }
    }

    public void PlayGodJobSound()
    {
        if (GoodJobClips != null)
        {
            audioSource.clip = GoodJobClips;
            audioSource.Play();
        }
    }

    public void PlayNextSound()
    {
        if (soundClips.Length > 0)
        {
            currentClipIndex = (currentClipIndex + 1) % soundClips.Length;
            PlaySound();
        }
    }
    public void FollowProcedure()
    {
        if (soundClips.Length > 0)
        {
            if (currentClipIndex == 0)
            {
                StartCoroutine(TrainingTimer());
                PlaySound();
                currentClipIndex = (currentClipIndex + 1) % soundClips.Length;
            }
            else
            {
                PlayGodJobSound();
                PlaySound();
                if (currentClipIndex == soundClips.Length - 1)
                {
                    isEnded = true;
                }
                currentClipIndex = (currentClipIndex + 1) % soundClips.Length;
            }
        }
    }

    public void PlayPreviousSound()
    {
        if (soundClips.Length > 0)
        {
            currentClipIndex = (currentClipIndex - 1 + soundClips.Length) % soundClips.Length;
            PlaySound();
        }
    }
    
    IEnumerator TrainingTimer()
    {
        Debug.Log("Timer Start");
        yield return new WaitForSeconds(limitTime);

        Debug.Log("Timer End");
        is5MinutesPassed = true;
    }
    
    void PlayEndTrainingSound()
    {
        if (endTrainingClip != null)
        {
            audioSource.clip = endTrainingClip;
            audioSource.Play();
        }
    }
}