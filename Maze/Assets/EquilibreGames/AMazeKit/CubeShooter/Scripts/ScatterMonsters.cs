using UnityEngine;
using System.Collections;
using EquilibreGames.AMazeKit;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class ScatterMonsters : MonoBehaviour
{
	
	public GameObject monster;
	public int minMonsterNumber = 0;
	public int maxMonsterNumber = 5;
	public static MazeRoom firstRoom = null;
	
	void AddMonster (MazeRoom room, int count) {
		Vector3 position = transform.position + Vector3.up * monster.GetComponent<Renderer>().bounds.extents.y;
		Vector3 direction = Vector3.right * monster.GetComponent<Renderer>().bounds.size.x;
		for (int i=0;i<count;i++)
			CreateMonster(room, position+direction*i);
	}
	
	void CreateMonster(MazeRoom room, Vector3 position) {
			// Position is determined by the center of the room, distance of the raycast hit - bounds of the crate
			// Bounds are also used to put it above the ground
			GameObject go = Instantiate (monster, position, Quaternion.identity) as GameObject;
			// Attach the crate to the room object
			if (room.roomObject != null)
				go.transform.parent = room.roomObject.transform;
			else {
				GameObject monsterRoot = GameObject.Find ("/Monsters");
				if (monsterRoot == null) {
					monsterRoot = new GameObject ("Monsters");
					monsterRoot.transform.position = Vector3.zero;
					monsterRoot.transform.rotation = Quaternion.identity;
				}
				go.transform.parent = monsterRoot.transform;
			}
	}
	
	// Used when maze is built to receive data about current room
	public void OnMazeUpdate(MazeRoom room) {
		if (firstRoom == null) {
			// If this is the first room ever, do not add crate, and store the first room
			// Note : this is used in multiple scripts in the demo, to optimize it, use only one static var !
			firstRoom = room;
			// Swith on light of the first room
			Light firstLight = room.roomObject.GetComponentInChildren<Light>();
			if (firstLight != null)
				firstLight.enabled = true;
			//Debug.Log("First Monster Room is " + room.roomObject.name);
			// If player is found, move it into this room (the room wihout any monster) and move the camera above him
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			if (player != null) {
				Vector3 abovePlayer = room.roomObject.transform.position;
				
				abovePlayer.y = player.transform.position.y;
				player.transform.position = abovePlayer;
				player.GetComponent<CSPlayer>().spawnPosition = abovePlayer;
				
				abovePlayer.y = Camera.main.transform.position.y;
				Camera.main.transform.position = abovePlayer;
			}
			return;
		}
		// raycast from the center at a random direction, and instantiate a crate
		int monsterCount = Random.Range(minMonsterNumber, maxMonsterNumber);
		AddMonster(room, monsterCount);
	}
}
