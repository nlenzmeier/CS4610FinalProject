using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	public int health = 5;
	public GameObject explosionPrefab;
	protected AudioSource myClip;
	
	void Start() {
		myClip = GetComponent<AudioSource>();
	}
	
	void Hurt() {
		health--;
		myClip.Play();
		if (health <= 0) {
#if UNITY_4_0 || UNITY_4_1
			gameObject.SetActive(false);
#else
			gameObject.SetActiveRecursively(false);
#endif			
			GameObject go = (GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation);
			Destroy(go, 2);
		}
	}
}
