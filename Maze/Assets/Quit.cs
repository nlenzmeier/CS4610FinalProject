using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour {

	private AudioSource m_audio_src;

	// Use this for initialization
	void Start () {
		m_audio_src = GetComponent<AudioSource>();
	}

	public void Button_OnClick()
	{
		Debug.Log("Quit Clicked");
		Application.Quit();
	}
}
