using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    public AudioClip audioClip;
    AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();

    }

    public void  PlayClearLineSound()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
