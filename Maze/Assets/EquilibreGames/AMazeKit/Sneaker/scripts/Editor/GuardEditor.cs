using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Guard))]
public class GuardEditor : Editor {

	void OnSceneGUI () {
		Guard g = target as Guard;
		
		Handles.color = Color.green;
		float direction = g.defaultVisionHeight;// / NGUITools.FindInParents<UIRoot>(g.gameObject).transform.localScale.x;
		Handles.Label(g.transform.position + Vector3.up, "Vision " + direction, EditorStyles.boldLabel);
		for (int i=0;i<g.patrol.Length;i++) {
			Vector3 pos = Handles.PositionHandle(g.patrol[i].position + g.transform.position, Quaternion.identity);
			// Y Lock
			pos.y = g.transform.position.y;
			// Z Lock
			pos.z = g.transform.position.z;
			g.patrol[i].position = pos - g.transform.position;
			Handles.Label(pos + Vector3.up * 0.1f, "WP " + i, EditorStyles.boldLabel);
		}
		if (GUI.changed) {
            EditorUtility.SetDirty (target);
        }
	}
}
