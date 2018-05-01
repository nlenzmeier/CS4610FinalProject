using UnityEngine;
using System.Collections;

public class SetupPlayer : MonoBehaviour {
	
	public GameObject playerPrefab;
	public Vector3 spawnPosition;
	public float defaultTime = 5;
	
	// Use this for initialization
	void OnMazeDone () {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		if (go == null)
			go = (GameObject) Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
		if (Camera.main.GetComponent<FollowPlayer>() == null)
			Camera.main.gameObject.AddComponent<FollowPlayer>();
		if (Camera.main.GetComponent<TimeSlider>() == null)
			Camera.main.gameObject.AddComponent<TimeSlider>();
		
		FollowPlayer fp = Camera.main.GetComponent<FollowPlayer>();
		fp.target = go.transform;
		fp.anyGuy = go.GetComponent<SneakerPlayer>();
		
		TimeSlider ts = Camera.main.GetComponent<TimeSlider>();
		ts.defaultTimeScale = defaultTime;
		ts.newTimeScale = ts.defaultTimeScale;
	}
}
