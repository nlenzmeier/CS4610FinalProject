using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Return : MonoBehaviour {

	// audio source reference
	private AudioSource m_audio_src;

    void Start()
	{
        m_audio_src = GetComponent<AudioSource>();
	}

	public void Button_OnClick()
	{
		Debug.Log("Return Clicked"); 
		//Delete (GameObject.Find ("Canvas"));
		m_audio_src.Play();
		SceneManager.LoadScene("Main", LoadSceneMode.Single);

	}
}
