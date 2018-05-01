using UnityEngine;
using System.Collections;

public class FadeMusic : MonoBehaviour {
	
	public float speed = 5;
	public bool running = false;
	public float target = 0;
	AudioSource source;
	
	void Awake() {
		source = GetComponent<AudioSource>();
		running = false;
	}
	
	void Update() {
		if (!running)
			return;
		
		source.volume += (target > 0 ? 1 : -1) * Time.deltaTime * speed;
		if (target >= 1 && source.volume >= 1) {
			running = false;
			source.volume = 1;
		} else if (target <= 0 && source.volume <= 0) {
			running = false;
			source.volume = 0;
		}
	}
	
	// Use this for initialization
	public void Enable () {
		if (target != 1) {
			target = 1;
			running = true;
		}
	}
	
	// Update is called once per frame
	public void Disable() {
		if (target != 0) {
			target = 0;
			running = true;
		}
	}
}
