using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Monster))]
public class MonsterInspector : Editor {

	void OnSceneGUI () {
		Monster m = target as Monster;
		
		Handles.color = Color.green;
		Handles.ArrowCap(1, m.transform.position, m.transform.rotation, 10);
		
		Handles.color = Color.red;
		Quaternion velocityQuat = new Quaternion();
		velocityQuat.SetFromToRotation(Vector3.forward, m.GetComponent<Rigidbody>().velocity);
		Handles.ArrowCap(1, m.transform.position, velocityQuat, 10);
	}
}
