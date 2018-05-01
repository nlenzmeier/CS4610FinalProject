using UnityEngine;
using System.Collections;

public class HandleTeleportation : MonoBehaviour {

	public float teleportOffset = 20.5f;
	public float teleportDelay = 0.25f;
	private float nextTeleportTime = 0;
	
	// Update is called once per frame
	void LateUpdate () {
		if (Input.GetButton("Fire1") && TeleportPlayer.currentTeleporter != null && TeleportPlayer.currentTeleporter.upAvailable) {
			if (Time.time > nextTeleportTime) {
				transform.Translate(Vector3.up * teleportOffset);
				nextTeleportTime = Time.time + teleportDelay;
			}
		}
		if (Input.GetButton("Fire2") && TeleportPlayer.currentTeleporter != null && TeleportPlayer.currentTeleporter.downAvailable) {
			if (Time.time > nextTeleportTime) {
				transform.Translate(Vector3.down * teleportOffset);
				nextTeleportTime = Time.time + teleportDelay;
			}
		}
	}
}
