using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	private float startTime;
	private bool finished = false;
	public Text HighScore;
	public float highScoreState; 

	// Use this for initialization
	void Start () {
		startTime = Time.time;	
		highScoreState = PlayerPrefs.GetFloat ("HighScore", -1);
	}
	
	// Update is called once per frame
	void Update () {
		if (finished)
			return;

		float t = Time.time - startTime;

		string minutes = ((int)t / 60).ToString("00");
        string seconds = "";
        if(t % 60 < 10)
        {
             seconds = (t % 60).ToString("f2");
             string appToMe = "0";
             seconds = appToMe + seconds;
        }
        else { seconds = (t % 60).ToString("f2"); }
        
		timerText.text = minutes + ":" + seconds;

		// initializes highScoreState to -1 IF there’s no saved high score. 
		// IF there is one, then 32 initializes highScoreState to the high score.
		if (highScoreState == -1) { 
			HighScore.text = "None"; 
		} else { 
			HighScore.text = getTimeStamp (highScoreState);
			//HighScore.text = highScoreState.ToString(); 
		}
	}

	public void Finish() {
		finished = true;
		timerText.color = Color.green;

		float highScoreState = PlayerPrefs.GetFloat("HighScore", -1);
		float newTime = Time.time - startTime;

		if (highScoreState == -1) { 
			PlayerPrefs.SetFloat("HighScore", newTime); 
		} else { 
			if (newTime < highScoreState) { 
				PlayerPrefs.SetFloat("HighScore", newTime); 
			} 
		}

		highScoreState = PlayerPrefs.GetFloat("HighScore", -1);

//		string minutes = ((int)highScoreState / 60).ToString();
//		string seconds = (highScoreState % 60).ToString("f2");

		//HighScore.text = PlayerPrefs.GetFloat("HighScore", 999).ToString();
		HighScore.text = getTimeStamp(PlayerPrefs.GetFloat("HighScore", 999)); 
	}

	public string getTimeStamp(float time) {
		string minutes = ((int)highScoreState / 60).ToString("00");
		string seconds = (highScoreState % 60).ToString("00");

		return minutes + ":" + seconds;
	}
}


	