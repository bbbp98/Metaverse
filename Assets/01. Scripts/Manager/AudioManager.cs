using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip mainBgm;
    public AudioClip battleBgm;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlayBgm(SceneType type)
    {
        AudioClip clip = null;

        switch (type)
        {
            case SceneType.MainScene:
                clip = mainBgm;
                break;
            case SceneType.StackScene:
            case SceneType.LizardRunScene:
                clip = battleBgm;
                break;
        }
        
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void StopBgm()
    {
        bgmSource.Stop();
    }
}
