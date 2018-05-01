using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		GameObject.Find("ThirdPersonController").SendMessage("Finish");
	}
}
