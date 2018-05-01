using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {

	public void Button_OnClick() {
		Debug.Log ("Play Clicked");
		//Delete (GameObject.Find ("Canvas"));
		SceneManager.LoadScene ("Scene0", LoadSceneMode.Single);

	}
}
