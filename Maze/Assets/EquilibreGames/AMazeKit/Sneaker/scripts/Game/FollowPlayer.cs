using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	
	public Transform target;
	public SneakerPlayer anyGuy;
	public Vector2 smoothTime = Vector2.one * 1.2f;
	public Vector2 lookingForward = Vector2.one;
	Vector2 velocity;
	Transform thisTransform;
	
	// Use this for initialization
	void Start () {
		thisTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = thisTransform.position;
		position.x = Mathf.SmoothDamp( thisTransform.position.x, target.position.x + anyGuy.horizontal * lookingForward.x, ref velocity.x, smoothTime.x);
		position.y = Mathf.SmoothDamp( thisTransform.position.y, target.position.y + lookingForward.y, ref velocity.y, smoothTime.y);
		thisTransform.position = position;
	}
}
