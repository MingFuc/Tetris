using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    public static bool BackgroundSound = false;
    public static bool EffectSound = true;

    public Toggle BMToggle;
    public Toggle EffectToggle;

    public AudioClip BackgroundSoundClip;
    public AudioClip SoundEffectClip;
    public AudioClip Lose_EffectClip;

    AudioSource MainAudioSource;
    AudioSource EffectAudioSource;
    AudioSource Lose_EffectAudioSource;

    private void Awake()
    {
        instance = this;

        BMToggle.isOn = BackgroundSound;
        BMToggle.onValueChanged.AddListener(Turn_On_Off_BMSound);

        EffectToggle.isOn = EffectSound;
        EffectToggle.onValueChanged.AddListener(Turn_On_Off_EffectSound);


        MainAudioSource = gameObject.AddComponent<AudioSource>();
        EffectAudioSource = gameObject.AddComponent<AudioSource>();
        Lose_EffectAudioSource = gameObject.AddComponent<AudioSource>();

        MainAudioSource.clip = BackgroundSoundClip;
        MainAudioSource.loop = true;
        MainAudioSource.playOnAwake = false;

        EffectAudioSource.clip = SoundEffectClip;
        EffectAudioSource.playOnAwake = false;

        Lose_EffectAudioSource.clip = Lose_EffectClip;
        Lose_EffectAudioSource.playOnAwake = false;

        MainBoard.instance.onPause += ChangeBMState;

        MainBoard.instance.onLose += PauseBackgroundMusic;
        MainBoard.instance.onLose += PlayLoseSound;


    }
    private void OnDestroy()
    {
        MainBoard.instance.onPause -= ChangeBMState;

        MainBoard.instance.onLose -= PauseBackgroundMusic;
        MainBoard.instance.onLose -= PlayLoseSound;
    }
    private void Start()
    {

        PlayBackgroundMusic();

    }

    public void PlayClearLineSound()
    {
        if (EffectSound == true)
        {
            EffectAudioSource.Play();
        }
    }

    public void PlayLoseSound()
    {
        if (EffectSound == true)
        {
            Lose_EffectAudioSource.Play();
        }
    }

    public void ChangeBMState()
    {

        if (MainBoard.instance.isGamePaused == true)
        {
            PlayBackgroundMusic();
        }
        else
        {
            PauseBackgroundMusic();
        }

    }

    void PauseBackgroundMusic()
    {
        if (BackgroundSound == true)
        {
            MainAudioSource.Pause();
        }
    }

    void PlayBackgroundMusic()
    {
        if (BackgroundSound == true)
        {
            MainAudioSource.Play();
        }
    }

    public void Turn_On_Off_BMSound(bool whatever)
    {
        BackgroundSound = !BackgroundSound;

    }
    
    public void Turn_On_Off_EffectSound(bool whatever)
    {
        EffectSound = !EffectSound;
    }

}
