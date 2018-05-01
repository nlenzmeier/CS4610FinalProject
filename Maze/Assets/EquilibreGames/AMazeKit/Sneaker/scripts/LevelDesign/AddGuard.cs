using UnityEngine;
using System.Collections;
using EquilibreGames.AMazeKit;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class AddGuard : MonoBehaviour {
	
	[System.Serializable]
	public class GuardType {
		public int minCount = 0;
		public int maxCount = 3;
		public GameObject prefab;
		public Vector3 minPosition;
		public Vector3 step;
	}

	public GuardType[] guards;
	
	// Used when maze is built to receive data about current room
	public void OnMazeUpdate(MazeRoom room) {
		// Add a guard
		int guardIndex = Random.Range(0, guards.Length);
		GuardType newGuard = guards[guardIndex];
		int count = Random.Range(newGuard.minCount, newGuard.maxCount);
		
		// start from Random(0, maxCount-count)
		Vector3 startPosition = newGuard.minPosition + newGuard.step*Random.Range(0, newGuard.maxCount-count);
		for (int i=0;i<count;i++) {
			Vector3 localPosition = startPosition + newGuard.step*i;
			GameObject go = (GameObject)Instantiate(newGuard.prefab, Vector3.zero, Quaternion.identity);
			go.transform.parent = room.roomObject.transform;
			go.transform.localPosition = localPosition;
			// Update patrol points from start position (take 0 as reference point)
			Guard g = go.GetComponent<Guard>();
			for (int p=0;p<g.patrol.Length;p++) {
				// substract position to patrol waypoint
				g.patrol[p].position -= localPosition;
				// modify speed a little
				float speedFactor = Random.Range(0.8f, 1.6f);
				g.patrol[p].speedToReach *= speedFactor;
				// adap sprite animation fps to speed (we know that's the animation at index 1, ugly hard coded thing...)
				g.spriteAnimation.animationList[1].fps *= speedFactor;
			}
		}	
	}
}
