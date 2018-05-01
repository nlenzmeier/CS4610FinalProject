using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetTime : MonoBehaviour {

	public void Reset() {
		Debug.Log ("Reset Clicked");
		PlayerPrefs.DeleteAll();
	}
}