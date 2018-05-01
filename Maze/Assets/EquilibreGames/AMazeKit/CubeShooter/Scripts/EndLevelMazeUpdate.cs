using UnityEngine;
using System.Collections;
using EquilibreGames.AMazeKit;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class EndLevelMazeUpdate : MonoBehaviour {
	
	public static MazeRoom lastRoom;
	public GameObject endLevelObject;
	public static GameObject endLevelInstance;
	
	// Use this for initialization
	void OnMazeUpdate (MazeRoom room) {
		lastRoom = room;
	}
	
	void OnEnable() {
		if (lastRoom == null || lastRoom.roomObject == null)
			return;
		if (endLevelInstance == null) {
			//endLevelInstance = (GameObject)Instantiate(endLevelObject, lastRoom.roomObject.transform.position, Quaternion.identity);
			endLevelInstance = GameObject.FindGameObjectWithTag("EndTrigger");
		}
		if (endLevelInstance != null)
			endLevelInstance.transform.position = lastRoom.roomObject.transform.position;
	}
}
