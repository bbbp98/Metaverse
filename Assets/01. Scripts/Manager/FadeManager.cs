using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    public Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1));
    }

    public void FadeToScene(SceneType sceneType)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(sceneType));
    }

    private IEnumerator FadeRoutine(SceneType sceneType)
    {
        // fade out
        yield return StartCoroutine(Fade(1));

        // load scene
        SceneManager.LoadScene(sceneType.ToString());

        // fade in
        yield return StartCoroutine(Fade(0));

        gameObject.SetActive(false);
    }

    /// <summary>
    /// value 0: fade-in, 1: fade-out
    /// </summary>
    public IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null)
        {
            Debug.Log("fade image is null");
            gameObject.SetActive(false);
            yield break;
        }


        Color color = fadeImage.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
}
