using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour {

    // reference to the character
    private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter m_cc;
    private AudioSource m_audio_src;

    // audio clips
    public AudioClip running_footstep;
    public AudioClip walking_footstep;

    // Use this for initialization
    void Start () {
        m_cc        = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();
        m_audio_src = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame 
	void Update () {
        if (m_cc.isRunning())
        {
            m_audio_src.clip = running_footstep;
            m_audio_src.volume = Random.Range(0.04f, 0.06f);
            //m_audio_src.volume = 1.0f;
        }
        if (m_cc.isWalking())
        {
            m_audio_src.clip = walking_footstep;
            m_audio_src.volume = Random.Range(0.02f, 0.04f);
        }

        // enable the audio
        if ((m_cc.isMoving() == true) && (m_cc.isGrounded() == true) && (m_audio_src.isPlaying == false))
        {
            // slightly alter pitch/volume so steps sound different
            m_audio_src.pitch  = Random.Range(0.85f, 1.15f);
            m_audio_src.Play();
        }
	}
}
