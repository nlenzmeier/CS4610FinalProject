using UnityEngine;
using System.Collections;

public class TimeSlider : MonoBehaviour {

	private float startTime;
	public static bool started = false;
	public float defaultTimeScale  = 0.01f;
	public float newTimeScale = 0.02f;
	public float delayToNewScale = 0.01f;
	public bool showGui = true;

	void Start() {
		Time.timeScale = defaultTimeScale;
		if (defaultTimeScale != newTimeScale) {
			startTime = Time.time + delayToNewScale;
			started = false;
		}
		else {
			started = true;
		}
	}

	void Update () {
		if (started)
			return;
		if (Time.time > startTime) {
			Time.timeScale = newTimeScale;
			started = true;
		}
	}

	// Use this for initialization
	void OnGUI () {
		if (!showGui)
			return;
		
		GUILayout.BeginArea(new Rect(10, 10, 200, 20));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Time Scale");
		Time.timeScale = GUILayout.HorizontalSlider(Time.timeScale, 0f, 10f, GUILayout.Width(100));
		Time.timeScale = Mathf.Round(Time.timeScale * 10) / 10;
		GUILayout.Label("" + Time.timeScale);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}
