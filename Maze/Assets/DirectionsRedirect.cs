using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectionsRedirect : MonoBehaviour {

	// audio source reference
	private AudioSource m_audio_src;

	void Start()
	{
		m_audio_src = GetComponent<AudioSource>();
	}

	public void Button_OnClick()
	{
		Debug.Log("Directions Clicked"); 
		//Delete (GameObject.Find ("Canvas"));
		m_audio_src.Play();
		SceneManager.LoadScene("Directions", LoadSceneMode.Single);

	}
}
