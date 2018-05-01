using UnityEngine;
using System.Collections;
using EquilibreGames.AMazeKit;

public class AddCar : MonoBehaviour {
	
	static MazeRoom firstRoom = null;
	public GameObject carPrefab;
	public Vector3 position;
	
	void OnMazeUpdate (MazeRoom room) {
		if (firstRoom == null) {
			firstRoom = room;
			GameObject go = (GameObject)Instantiate(carPrefab, transform.position + position, Quaternion.identity);
			go.transform.parent = room.roomObject.transform;
		}
	}
}
