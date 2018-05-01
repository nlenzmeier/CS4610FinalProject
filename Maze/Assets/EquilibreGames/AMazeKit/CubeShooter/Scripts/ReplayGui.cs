using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using EquilibreGames.AMazeKit;

public class ReplayGui : MonoBehaviour
{
	
	public MazeData maze;
	private Texture2D whiteTex;
	
	void Start ()
	{
		whiteTex = new Texture2D (1, 4);
		whiteTex.SetPixel (1, 1, Color.grey);
		whiteTex.SetPixel (1, 2, Color.white);
		whiteTex.SetPixel (1, 3, Color.grey);
		whiteTex.SetPixel (1, 4, Color.grey);
		whiteTex.Apply ();
		
		maze = GameObject.Find ("aMaze").GetComponent<MazeData> ();
		
		CreateNewMaze (3, 1, 3);
	}

	void CreateNewMaze (int x, int y, int z)
	{
		ScatterCrates.firstRoom = null;
		ScatterMonsters.firstRoom = null;
		maze.maze.Reset ();
		
		// Set algorithm parameters
		DepthFirstSearchPrefs prefs = new DepthFirstSearchPrefs ();
		prefs.openness = 50;
		prefs.roomChoice = DepthFirstSearchGenerator.Choice.random;
		prefs.size = new Vector3 (x, y, z);
		
		// Run the algorithm :
		// create the algorithm iterator object
		DepthFirstSearchGenerator dfs = new DepthFirstSearchGenerator ();
		// initialize from data (return step count)
		dfs.Initialize (maze.maze, prefs);
		// step to create rooms
		while (dfs.Step())
			;
		
		/*
		 // you could constraint to the first N rooms (ex: 50) to create a maze that is not square shaped with this algorithm (openness parameter is used in the last steps though)
			int stepCount = Mathf.Min(dfs.Initialize (maze.maze, prefs), 50);
			for (int i=0;i<stepCount;i++)
				dfs.Step();
		// */
		
		// end, clear the data and return the maze description
		maze.maze = dfs.End ();
		
		// Build Game objects from maze description
		BuildLabyrinth (maze);
	}
	
	// Update is called once per frame
	void OnGUI ()
	{
		GUI.color = new Color (0, 0, 0, 0.5f);
		GUI.DrawTexture (new Rect (Screen.width - 140, 4, 138, 78), whiteTex);
		GUI.color = Color.white;
		if (GUI.Button (new Rect (Screen.width - 138, 6, 134, 22), "Tiny maze (9)")) {
			CreateNewMaze (3, 1, 3);
		}
		if (GUI.Button (new Rect (Screen.width - 138, 32, 134, 22), "Standard maze (63)")) {
			CreateNewMaze (7, 1, 9);
		}
		if (GUI.Button (new Rect (Screen.width - 138, 58, 134, 22), "Big maze (165)")) {
			CreateNewMaze (11, 1, 15);
		}
	}
	
	static GameObject CreateMazeRoom (Vector3 coords, GameObject theCube, GameObject prefab, Vector3 prefabPosition)
	{
		GameObject myMazeRoom = Instantiate (prefab) as GameObject;
		myMazeRoom.transform.position = prefabPosition;
		myMazeRoom.transform.rotation = prefab.transform.rotation;
		myMazeRoom.name = "Room_" + coords.x.ToString () + "_" + coords.y.ToString () + "_" + coords.z.ToString ();
		myMazeRoom.transform.parent = theCube.transform;
		return myMazeRoom;
	}

	public static bool RemoveMazeChildren (GameObject theCube)
	{

		for (int i=theCube.transform.childCount-1; i>=0; i--) {
			DestroyImmediate (theCube.transform.GetChild (i).gameObject);
		}
		return true;
	}
	
	public static void BuildLabyrinth (MazeData mazeData)
	{
		MazeRoomPrefab[] roomPrefabs = mazeData.roomPrefabList.prefabs;
		// Initialize room bounds
		Bounds maxBounds = new Bounds (Vector3.zero, Vector3.zero);
		for (int i=0; i<roomPrefabs.Length; i++) {
			maxBounds = MazeRoomUtility.GetMaxBounds (maxBounds, roomPrefabs [i]);
		}
		//
		GameObject theCube = mazeData.gameObject;
		MazeDescription maze = mazeData.maze;
		
		// Remove all rooms (children of the maze game object)
		if (!RemoveMazeChildren (theCube))
			return;
		
		for (int i = 0; i < maze.Rooms.Count; i++) {
			MazeRoom mazeRoom = maze.Rooms [i];
			Vector3 prefabPosition = new Vector3 (mazeRoom.coords.x * maxBounds.size.x, mazeRoom.coords.y * maxBounds.size.y, mazeRoom.coords.z * maxBounds.size.z);
				
			if (mazeRoom != null && roomPrefabs != null && roomPrefabs.Length > mazeRoom.OpenedWays && roomPrefabs [mazeRoom.OpenedWays] != null && roomPrefabs [mazeRoom.OpenedWays].rooms != null && roomPrefabs [mazeRoom.OpenedWays].rooms.Count > 0) {
				GameObject myMazeRoom = CreateMazeRoom (mazeRoom.coords, theCube, roomPrefabs [mazeRoom.OpenedWays].rooms [0], prefabPosition);
				mazeRoom.roomObject = myMazeRoom;
				// Send a signal to the room so it can handle its state if needed (ex: teleporter, scripts as ScatterMonster, ScatterCrates)
				myMazeRoom.BroadcastMessage ("OnMazeUpdate", mazeRoom, SendMessageOptions.DontRequireReceiver);
			} else {
				if (roomPrefabs.Length > mazeRoom.OpenedWays) {
					Debug.LogWarning ("No prefab for Room(" + i + ") " + mazeRoom.OpenedWays);
				} else if (mazeRoom == null) {
					Debug.LogError ("Found a null Room@ " + i + " !");
				} else {
					Debug.LogWarning ("Wrong prefab request@" + i + " " + mazeRoom.coords + " :" + mazeRoom.OpenedWays);
				}
			}
		}
		// Send the end signal to all rooms
		mazeData.BroadcastMessage ("OnMazeDone", mazeData, SendMessageOptions.DontRequireReceiver);

	}
}
