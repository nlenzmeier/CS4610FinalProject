using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	private float endTime;
	public float liveDuration = 2;
	
	void Start() {
		endTime = Time.time + liveDuration;
	}
	
	void Update() {
		if (Time.time > endTime)
			Destroy(gameObject);
	}
	
	void OnCollisionEnter(Collision collision) {
		collision.gameObject.SendMessage("Hurt", SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}

}
