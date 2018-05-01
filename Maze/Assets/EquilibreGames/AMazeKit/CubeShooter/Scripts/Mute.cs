using UnityEngine;
using System.Collections;

public class Mute : MonoBehaviour {

	void OnGUI () {
		bool muted = (AudioListener.volume == 0);
		string title = (muted ? "Unmute" : "Mute");
		if (GUI.Button(new Rect(4,Screen.height-24, 80, 20), title)) {
			AudioListener.volume = (muted ? 1 : 0);
		}
	}
}
