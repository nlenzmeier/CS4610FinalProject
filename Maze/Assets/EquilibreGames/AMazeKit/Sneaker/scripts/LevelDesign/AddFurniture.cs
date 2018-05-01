using UnityEngine;
using System.Collections;
using EquilibreGames.AMazeKit;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class AddFurniture : MonoBehaviour {
	
	[System.Serializable]
	public class Furniture {
		public int minCount = 0;
		public int maxCount = 3;
		public GameObject prefab;
		public Vector3 minPosition;
		public Vector3 step;
	}

	public Furniture[] furnitures;
	
	// Used when maze is built to receive data about current room
	public void OnMazeUpdate(MazeRoom room) {
		// Add a furniture
		int furnitureIndex = Random.Range(0, furnitures.Length);
		Furniture newFurniture = furnitures[furnitureIndex];
		int count = Random.Range(newFurniture.minCount, newFurniture.maxCount);
		
		// start from Random(0, maxCount-count)
		Vector3 startPosition = newFurniture.minPosition + newFurniture.step*Random.Range(0, newFurniture.maxCount-count);
		for (int i=0;i<count;i++) {
			Vector3 localPosition = startPosition + newFurniture.step*i;
			GameObject go = (GameObject)Instantiate(newFurniture.prefab, Vector3.zero, Quaternion.identity);
			go.transform.parent = room.roomObject.transform;
			go.transform.localPosition = localPosition;
		}	
	}
}
