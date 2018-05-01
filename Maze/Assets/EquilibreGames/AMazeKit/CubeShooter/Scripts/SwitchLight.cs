using UnityEngine;
using System.Collections;

public class SwitchLight : MonoBehaviour {
	
	public Light myLight;
	Color[] mainColors;
	//Renderer roomRender;
	
	void Awake() {
		if (myLight == null)
			myLight = GetComponentInChildren<Light>();
		/*
		roomRender = transform.parent.renderer;
		mainColors = new Color[roomRender.materials.Length];
		for (int i=0;i<roomRender.materials.Length;i++) {
			mainColors[i] = roomRender.materials[i].color;
			roomRender.materials[i].color = Color.black;
		}
		// */
		myLight.enabled = false;
	}
	
	// Use this for initialization
	void OnTriggerEnter (Collider collider) {
		if (collider.CompareTag("Player")) {
			//SwitchOn();
			myLight.enabled = true;
		}
	}
	
	/*
	void SwitchOn() {
		Debug.Log ("Switch on " + gameObject.name);
		Debug.Log ("Parent renderer has " + roomRender.materials.Length + " materials");
		for (int i=0;i<roomRender.materials.Length;i++) {
			Material vertexLitMat = roomRender.materials[i];
			vertexLitMat.color = Color.white;
		}
	}
	// */
}
