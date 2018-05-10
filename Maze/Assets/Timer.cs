using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	private float startTime;
	private float elapsedTime;
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

		elapsedTime = Time.time - startTime;	//cache time so it is consistent throughout code
		timerText.text = getTimeStamp(Time.time - startTime);

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
		//float newTime = Time.time - startTime;

		if (highScoreState == -1) { 
			PlayerPrefs.SetFloat("HighScore", elapsedTime); 
		} else { 
			if (elapsedTime < highScoreState) { 
				PlayerPrefs.SetFloat("HighScore", elapsedTime); 
			} 
		}

		highScoreState = PlayerPrefs.GetFloat("HighScore", -1);

		HighScore.text = getTimeStamp(PlayerPrefs.GetFloat("HighScore", 999)); 
	}

	public string getTimeStamp(float time) {
		string minutes = ((int)time / 60).ToString("00");
		string seconds = "";
		if(time % 60 < 10)
		{
			seconds = (time % 60).ToString("f2");
			string appToMe = "0";
			seconds = appToMe + seconds;
		}
		else { seconds = (time % 60).ToString("f2"); }

		return minutes + ":" + seconds;
	}
}


	