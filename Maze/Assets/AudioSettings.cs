using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour {

    private bool audio_is_on;

	// Use this for initialization
	void Start () {
        audio_is_on = false;
        Object.DontDestroyOnLoad(gameObject);
    }

    public void SetAudio( bool b )
    {
        audio_is_on = b;
    }

    public bool AudioIsOn()
    {
        return ( audio_is_on );
    }
}
