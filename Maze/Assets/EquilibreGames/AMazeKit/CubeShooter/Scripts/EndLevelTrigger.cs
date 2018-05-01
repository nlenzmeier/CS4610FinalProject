using UnityEngine;
using System.Collections;
using EquilibreGames.AMazeKit;

public class EndLevelTrigger : MonoBehaviour {
	
	public HideDelayed endInstruction;
	
	// Use this for initialization
	void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag("Player")) {
			endInstruction.Show();
		}
	}
}
