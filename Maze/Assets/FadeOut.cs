/*
Attach this to an object with an AudioSource to have it
stick around and then fade out when we switch scenes.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    #region Public Properties

    public float fadeOutTime = 3f;

    #endregion
    //--------------------------------------------------------------------------------
    #region Private Properties

    AudioSource audioSource;
    bool fading;
    float fadePerSec;

    #endregion
    //--------------------------------------------------------------------------------
    #region MonoBehaviour Events
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Assert(audioSource != null);

        Object.DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (fading)
        {
            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume, 0, fadePerSec * Time.deltaTime);
        }
    }

    #endregion
    //--------------------------------------------------------------------------------
    #region Private Methods

    void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
    {
        fading = true;
        fadePerSec = audioSource.volume / fadeOutTime;
        Destroy(gameObject, fadeOutTime);
    }

    #endregion
}