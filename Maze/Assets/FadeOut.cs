using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    public float fadeOutTime = 3f;
    public string tag_str = "";

    AudioSource audioSource;
    bool fading;
    float fadePerSec;
    string thisScene = "";

    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag_str);
        if (objs.Length > 1)
            Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
        Debug.Assert(audioSource != null);
        audioSource.Play();

        DontDestroyOnLoad(gameObject);
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
    
    void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
    {
        if( ( loadedScene.name != "Directions" ) && ( loadedScene.name != thisScene ) )
        {
            fading = true;
            fadePerSec = audioSource.volume / fadeOutTime;
            Destroy(gameObject, fadeOutTime);
        }
    }

}