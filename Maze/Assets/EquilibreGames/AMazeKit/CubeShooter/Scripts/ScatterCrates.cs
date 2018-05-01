using UnityEngine;
using System.Collections;
using EquilibreGames.AMazeKit;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class ScatterCrates : MonoBehaviour
{
	
	public GameObject crate;
	public int minCrateNumber = 0;
	public int maxCrateNumber = 5;
	public static MazeRoom firstRoom = null;

	void AddCrate (MazeRoom room, int count)
	{
		int randomAngle = Random.Range (30, 60);
		// Check on 4 different angles the further hit distance
		int rotationAngle = randomAngle;
		float rotationDistance = room.roomObject.GetComponent<Renderer>().bounds.size.magnitude*2;
		
		float distance = CheckAngle(randomAngle, crate.GetComponent<Renderer>().bounds.extents.y);
		rotationDistance = distance;
		distance = CheckAngle(randomAngle+90, crate.GetComponent<Renderer>().bounds.extents.y);
		if (distance < rotationDistance && distance < Mathf.Infinity) {
			rotationDistance = distance;
			rotationAngle = randomAngle+90;
		}
		distance = CheckAngle(randomAngle+180, crate.GetComponent<Renderer>().bounds.extents.y);
		if (distance < rotationDistance && distance < Mathf.Infinity) {
			rotationDistance = distance;
			rotationAngle = randomAngle+180;
		}
		distance = CheckAngle(randomAngle+270, crate.GetComponent<Renderer>().bounds.extents.y);
		if (distance < rotationDistance && distance < Mathf.Infinity) {
			rotationDistance = distance;
			rotationAngle = randomAngle+270;
		}
		
		Vector3 direction = Quaternion.Euler (0, rotationAngle, 0) * Vector3.forward;
		Vector3 centralPosition = transform.position + (rotationDistance - crate.GetComponent<Renderer>().bounds.size.magnitude*2f) * direction + Vector3.up * crate.GetComponent<Renderer>().bounds.extents.y;
		
		Vector3 upsized = crate.GetComponent<Renderer>().bounds.size * 1.2f;
		//Debug.Log("Scatter Crate from " + centralPosition + " rotDist " + rotationDistance + " for room " + room.coords + " random angle was " + randomAngle + " at Y = " + crate.renderer.bounds.extents.y);
		if (rotationDistance != Mathf.Infinity) {
			/*
			Quaternion rotation = Quaternion.Euler (0, randomAngle, 0);
			Vector3 rayDirection = rotation * Vector3.forward * 100;
			Debug.DrawRay(transform.position + Vector3.up * crate.renderer.bounds.extents.y, rayDirection, Color.red, 30);
			rotation = Quaternion.Euler (0, randomAngle+90, 0);
			rayDirection = rotation * Vector3.forward * 100;
			Debug.DrawRay(transform.position + Vector3.up * crate.renderer.bounds.extents.y, rayDirection, Color.green, 30);
			rotation = Quaternion.Euler (0, randomAngle+180, 0);
			rayDirection = rotation * Vector3.forward * 100;
			Debug.DrawRay(transform.position + Vector3.up * crate.renderer.bounds.extents.y, rayDirection, Color.yellow, 30);
			rotation = Quaternion.Euler (0, randomAngle+270, 0);
			rayDirection = rotation * Vector3.forward * 100;
			Debug.DrawRay(transform.position + Vector3.up * crate.renderer.bounds.extents.y, rayDirection, Color.blue, 30);
			//*/
		
			for (int i=0;i<count;i++) {
				Vector3 cratePosition = centralPosition;
				switch (i) {
				case 1 : cratePosition += upsized.x * Vector3.right + upsized.z * Vector3.forward / 2; break;
				case 2 : cratePosition += upsized.x * Vector3.right / 2 - upsized.z * Vector3.forward;  break;
				case 3 : cratePosition += upsized.z * Vector3.forward; break;
				case 4 : cratePosition += upsized.x * Vector3.left; break;
				}
				CreateCrate (room, Quaternion.Euler (0, Random.Range(-5, 5), 0) , cratePosition);
			}
		}
	}
	
	float CheckAngle(int angle, float yOffset) {
		Quaternion rotation = Quaternion.Euler (0, angle, 0);
		Vector3 direction = rotation * Vector3.forward;
		Ray ray = new Ray (transform.position + Vector3.up * yOffset, direction);
		
		RaycastHit raycastInfo;
		
		if (Physics.Raycast (ray, out raycastInfo, 100)) {
			// Position is determined by the center of the room, distance of the raycast hit - bounds of the crate
			// Bounds are also used to put it above the ground
			// Put it only if it's far from the center (two times the size of the crate at least)
			float hitDistance = (raycastInfo.point - transform.position).magnitude;
			if (hitDistance > (2 * crate.GetComponent<Renderer>().bounds.size.magnitude)) {
				return hitDistance;
			}
		}
		return Mathf.Infinity;
	}

	void CreateCrate (MazeRoom room, Quaternion rotation, Vector3 position)
	{
		GameObject go = Instantiate (crate, position, rotation) as GameObject;
		// Attach the crate to the room object
		if (room.roomObject != null)
			go.transform.parent = room.roomObject.transform;
		else {
			GameObject crateRoot = GameObject.Find ("/Crates");
			if (crateRoot == null) {
				crateRoot = new GameObject ("Crates");
				crateRoot.transform.position = Vector3.zero;
				crateRoot.transform.rotation = Quaternion.identity;
			}
			go.transform.parent = crateRoot.transform;
		}
	}
	
	// Used when maze is built to receive data about current room
	public void OnMazeUpdate(MazeRoom room) {
		int crateCount = Random.Range(minCrateNumber, maxCrateNumber);
		if (firstRoom == null) {
			// If this is the first room ever, do not add crate, and store the first room
			// Note : this is used in multiple scripts in the demo, to optimize it, use only one static var !
			firstRoom = room;
			crateCount = 5;
			//Debug.Log("First Crate Room is " + room.roomObject.name);
			//return;
		}
		// raycast from the center at a random direction, and instantiate a crate
		AddCrate(room, crateCount);
	}
	
	public void OnMazeDone(MazeData mazeData) {
		if (firstRoom != null) {
			// Place the player at the center of the first room
			if (GameObject.FindGameObjectWithTag("Player") != null) {
				Transform playerT = GameObject.FindGameObjectWithTag("Player").transform;
				if (playerT != null)
					playerT.position = Vector3.Scale(mazeData.roomSize, firstRoom.coords) + playerT.localScale.y*0.5f * Vector3.up;
			}
		}
	}
}
