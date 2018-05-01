using UnityEngine;
using System.Collections;
 
public class DontGoThroughThings : MonoBehaviour
{
	public LayerMask layerMask; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed 
 
	private float minimumExtent;
	private float partialExtent;
	private float sqrMinimumExtent;
	private Vector3 previousPosition;
	public Transform objectRoot; 
	public Collider objectCollider;
	private Vector3 offset;
 
	//initialize values 
	void Awake ()
	{ 
		minimumExtent = Mathf.Min (Mathf.Min (objectCollider.bounds.extents.x, objectCollider.bounds.extents.y), objectCollider.bounds.extents.z); 
		partialExtent = minimumExtent * (1.0f - skinWidth); 
		sqrMinimumExtent = minimumExtent * minimumExtent;
		InitPosition();
	}
	
	public void InitPosition() {
		previousPosition = objectCollider.bounds.center;
		offset = (objectCollider.bounds.center - objectRoot.transform.position);
	}
 
	void FixedUpdate ()
	{
		//have we moved more than our minimum extent? 
		Vector3 movementThisStep = objectCollider.bounds.center - previousPosition; 
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;
 
		if (movementSqrMagnitude > sqrMinimumExtent) { 
			float movementMagnitude = Mathf.Sqrt (movementSqrMagnitude);
			RaycastHit hitInfo; 
 
			//check for obstructions we might have missed 
			Debug.DrawRay (previousPosition, movementThisStep * movementMagnitude, Color.red);
			if (Physics.Raycast (previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))  {
				Debug.DrawLine (previousPosition + Vector3.right, hitInfo.point + Vector3.right, Color.yellow, 10);
				Debug.Log ("Hit " + hitInfo.transform.name);
				objectRoot.position = (hitInfo.point - offset) - (movementThisStep / movementMagnitude) * partialExtent;
			}
			previousPosition = objectCollider.bounds.center;
		}
 
	}
}