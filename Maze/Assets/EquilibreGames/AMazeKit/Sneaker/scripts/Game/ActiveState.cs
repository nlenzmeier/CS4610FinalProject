using UnityEngine;

public static class ActiveState {

	public static void SetActive(GameObject gameObject, bool state) {
		// For Unity 3.4 and 3.5
#if UNITY_3_4 || UNITY_3_5
		gameObject.SetActiveRecursively(state);
#else
		// For Unity 4.x
		gameObject.SetActive(state);
#endif
	}	
}
