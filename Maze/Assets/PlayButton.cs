using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    // audio source reference
    private AudioSource m_audio_src;

    void Start()
    {
        m_audio_src = GetComponent<AudioSource>();
    }

    public void Button_OnClick()
    {
        Debug.Log("Play Clicked");
        //Delete (GameObject.Find ("Canvas"));
        m_audio_src.Play();
        SceneManager.LoadScene("Scene0", LoadSceneMode.Single);

    }
}
