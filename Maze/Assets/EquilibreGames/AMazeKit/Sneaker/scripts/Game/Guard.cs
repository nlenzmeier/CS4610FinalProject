using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour {
	
	[System.Serializable]
	public class Waypoint {
		public Vector3 position;
		public Vector3 calculatedPosition;
		public float pauseWhenArrived;
		public float speedToReach;
	}
	
	public LayerMask layerMask; //make sure we aren't in this layer 
	public GameObject sprite;
	public SimpleSpriteAnimation spriteAnimation;
	public enum GuardStatus {idle, moving, sleeping, warning, alert};
	GuardStatus status = GuardStatus.idle;
	float nextMove = 0;
	public float nearThreshold = 1f;
	public Waypoint[] patrol;
	Vector3 direction;
	int currentWaypoint = 0;
	bool orientationChanged = false;
	RaycastHit[] hitInfoList;
	
	#region vision
	
	public Color normalVision;
	public Color alertVision;
	
	//UIRoot root;
	public MeshFilter visionMeshFilter;
	public MeshRenderer visionMeshRenderer;
	Mesh coneDeVision = null;
	public Vector3 visionOffset;
	public float defaultVisionLength = 192;
	float visionLength = 192f;
	public float defaultVisionHeight = 64;
	float visionHeight = 64;
	#endregion
	
	// Use this for initialization
	void Start () {
		currentWaypoint = 0;
		if (patrol.Length == 0) {
			enabled = false;
			return;
		}
		// Calculate position for patrol points from transform.position
		for (int i=0;i<patrol.Length;i++)  {
			patrol[i].calculatedPosition = patrol[i].position + transform.position;
		}
		
		
		normalVision = visionMeshRenderer.sharedMaterial.GetColor("_EmisColor");
		//normalVision = visionMeshRenderer.sharedMaterial.GetColor("_TintColor");
		
		coneDeVision = CreateSprite.CreateTriangle();
		visionMeshFilter.sharedMesh = coneDeVision;
		status = GuardStatus.idle;
		nextMove = 0;
		visionLength = defaultVisionLength;
		visionHeight = defaultVisionHeight;
		direction = (patrol[0].calculatedPosition - transform.position).normalized;
		orientationChanged = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (status == GuardStatus.alert && GlobalHandler.Instance.player.status != SneakerPlayer.PlayerStatus.busted) {
			visionMeshRenderer.material.SetColor("_EmisColor", normalVision);
			status = GuardStatus.idle;
		}
		if (status == GuardStatus.idle && Time.time > nextMove) {
			status = GuardStatus.moving;
			// Set sprite animation
			spriteAnimation.SetAnimation("walk");
		}
		if (status == GuardStatus.moving) {
			if (!orientationChanged) {
				direction = (patrol[currentWaypoint].calculatedPosition - transform.position).normalized;
				Vector3 scale = sprite.transform.localScale;
				scale.x = Mathf.Abs(scale.x) * direction.x;
				sprite.transform.localScale = scale;
				orientationChanged = true;
			}
			// Draw line between here and destination
			Debug.DrawLine(transform.position, patrol[currentWaypoint].calculatedPosition, Color.red);
			transform.Translate(direction * patrol[currentWaypoint].speedToReach * Time.deltaTime);
			
			//Debug.Log ("Approaching next wp : " + (patrol[currentWaypoint].position-transform.position).sqrMagnitude);
			if ((direction.x < 0 && patrol[currentWaypoint].calculatedPosition.x > transform.position.x) || (direction.x > 0 && patrol[currentWaypoint].calculatedPosition.x < transform.position.x)) {
				// Pause when arrived
				nextMove = Time.time + patrol[currentWaypoint].pauseWhenArrived;
				status = GuardStatus.idle;
				// Set sprite (not animated)
				spriteAnimation.SetAnimation("idle");
				// Set next waypoint
				currentWaypoint++;
				if (currentWaypoint >= patrol.Length)
					currentWaypoint = 0;
				orientationChanged = false;
				//Debug.Log ("Setting next waypoint<" + currentWaypoint + "> : " + patrol[currentWaypoint].position + " direction " + direction);
			}
		}
		// Vision : raycast up/down
		//Vector3 visionDirection = Vector3.right * direction.x * defaultVisionLength / (root.transform.localScale.x*Camera.main.orthographicSize);
		Vector3 visionDirection = Vector3.right * direction.x * defaultVisionLength;
		Ray visionRay = new Ray(transform.position + Vector3.Scale(new Vector3(direction.x, 1, 1), visionOffset), visionDirection);
		
		Debug.DrawRay(visionRay.origin, visionRay.direction*defaultVisionLength, Color.cyan, 2f);
		//Debug.DrawLine(transform.position + coneDeVision.vertices[0], transform.position + coneDeVision.vertices[1], Color.yellow);
		//Debug.DrawLine(transform.position + coneDeVision.vertices[0], transform.position + coneDeVision.vertices[2], Color.yellow);
		//Debug.DrawLine(transform.position + Vector3.Scale(coneDeVision.vertices[0], root.transform.localScale), transform.position + Vector3.Scale(coneDeVision.vertices[1], root.transform.localScale), Color.yellow);
		//Debug.DrawLine(transform.position + Vector3.Scale(coneDeVision.vertices[0], root.transform.localScale), transform.position + Vector3.Scale(coneDeVision.vertices[2], root.transform.localScale), Color.yellow);
		
		hitInfoList = Physics.RaycastAll(visionRay);
		bool hit = false;
		float distance = defaultVisionLength + 1;
		Collider nearestUsefulCollider = null;
		
		if (hitInfoList.Length > 0) {
			for (int i=0;i<hitInfoList.Length;i++) {
				RaycastHit hitInfo = hitInfoList[i];
				// Do not take into account triggers
				if (hitInfo.collider.isTrigger)
					continue;
				// Do not take into account guards
				if (hitInfo.collider.GetComponent<Guard>() != null)
					continue;
				
				// Is hit distance closer than previous one and in range ?
				if (hitInfo.distance < distance && hitInfo.distance <= defaultVisionLength) {
					nearestUsefulCollider = hitInfo.collider;
					distance = hitInfo.distance;
					hit = true;
				}
			}
		}
		// If we hit something, check the nearest collider
		if (hit) {
			// Check if player is hidden or.. busted !
			if (nearestUsefulCollider.CompareTag("Player")) {
				if (GlobalHandler.Instance.player.status == SneakerPlayer.PlayerStatus.hidden) {
					hit = false;
				} else if (GlobalHandler.Instance.player.status != SneakerPlayer.PlayerStatus.busted) {
					// Say to the player he's busted !
					GlobalHandler.Instance.player.status = SneakerPlayer.PlayerStatus.busted;
					SneakerPlayer.mySecretPlace = null;
					status = GuardStatus.alert;
					visionMeshRenderer.material.SetColor("_EmisColor", alertVision);
					// Guard stops
					spriteAnimation.SetAnimation("idle");
				}
			}			
		}
		if (!hit) {
				visionLength = defaultVisionLength;
				visionHeight = defaultVisionHeight;
		} else {
			visionLength = distance;
			visionHeight = defaultVisionHeight * visionLength / defaultVisionLength;
		}
	}
	
	void LateUpdate() {
		UpdateVision();
	}
	
	void UpdateVision() {
		Vector3[] vertices = coneDeVision.vertices;
		
		vertices[0] = visionOffset;

		//vertices[1] = visionOffset + Vector3.right * direction.x * visionLength / (root.transform.localScale.x) + Vector3.up * visionHeight / (root.transform.localScale.y);
		//vertices[2] = visionOffset + Vector3.right * direction.x * visionLength / (root.transform.localScale.x) - Vector3.up * visionHeight / (root.transform.localScale.y);
		vertices[1] = visionOffset + Vector3.right * visionLength + Vector3.up * visionHeight;
		vertices[2] = visionOffset + Vector3.right * visionLength - Vector3.up * visionHeight;
		//vertices[1] = visionOffset + Vector3.right * direction.x * visionLength + Vector3.up * visionHeight;
		//vertices[2] = visionOffset + Vector3.right * direction.x * visionLength - Vector3.up * visionHeight;

		coneDeVision.vertices = vertices;
		coneDeVision.RecalculateNormals();
		coneDeVision.RecalculateBounds();
		;
	}
}
