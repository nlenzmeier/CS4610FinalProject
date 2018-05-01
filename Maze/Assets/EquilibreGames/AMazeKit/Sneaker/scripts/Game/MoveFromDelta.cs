using UnityEngine;
using System.Collections;

public class MoveFromDelta : MonoBehaviour {
	
	public Vector3 deltaRatio = Vector3.one;
	private Vector3 startPosition;
	public Transform targetTransform;
	private Vector3 startTargetPosition;
	
	// Use this for initialization
	void Awake () {
		startTargetPosition = targetTransform.position;
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = startPosition + Vector3.Scale(targetTransform.position - startTargetPosition, deltaRatio);
	}
}
