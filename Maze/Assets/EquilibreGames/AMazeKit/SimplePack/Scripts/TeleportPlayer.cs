using UnityEngine;
using EquilibreGames.AMazeKit;
using System.Collections;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class TeleportPlayer : MonoBehaviour {
	
	public bool upAvailable = false;
	public bool downAvailable = false;
	public int floor = 1;
	public float teleportMovement = 0;
	private TextMesh floorText;
	public GameObject player;
	public static TeleportPlayer currentTeleporter = null;
	
	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		if (GetComponent<Renderer>() != null)
			teleportMovement = GetComponent<Renderer>().bounds.size.y;
		floorText = GetComponentInChildren<TextMesh>();
		SetFloorText();
	}
	
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject != player)
			return;
		currentTeleporter = this;
	}
	
	void OnTriggerExit(Collider collider) {
		if (collider.gameObject != player)
			return;
		if (currentTeleporter == this)
			currentTeleporter = null;
	}
	
#if UNITY_EDITOR
	// Used when maze is built to receive data about current room
	public void OnMazeUpdate(MazeRoom room) {
		upAvailable = room.posY;
		downAvailable = room.negY;
		floor = Mathf.RoundToInt(room.coords.y);
	}
#endif
	
	public void SetFloorText() {
		if (floorText != null) {
			floorText.text = "" + floor;
		}
	}
}
