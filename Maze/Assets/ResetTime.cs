using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetTime : MonoBehaviour
{
    // audio source reference
    private AudioSource m_audio_src;

    void Start()
    {
        m_audio_src = GetComponent<AudioSource>();
    }

    public void Reset()
    {
        Debug.Log("Reset Clicked");
        m_audio_src.Play();
        PlayerPrefs.DeleteAll();
    }
}
