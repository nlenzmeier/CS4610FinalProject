using UnityEngine;
using System.Collections;

public class SneakerPlayer : MonoBehaviour {
	
	public enum PlayerStatus {playing, busted, arrived, onCopier, hidden};
	
	public PlayerStatus status = PlayerStatus.playing;
	public Vector3 offset = Vector3.up;
	public LayerMask layerMask; //make sure we aren't in this layer 
	public float rayLength = 0.4f;
	public GameObject sprite;
	public float speed = 1f;
	public bool fullSpeedOnly = true;
	RaycastHit hitInfo;
	public int horizontal = 0;
	public float startHelpDelay = 0f;
	bool hasMovedRight = false;
	bool hasMovedLeft = false;
	public bool displayedLeftHelp = false;
	public bool displayedRightHelp = false;
	Vector3 spawnPoint;
	public static GameObject mySecretPlace;
	public SimpleSpriteAnimation spriteAnimation;
	public bool searchedDesk = false;
	public FadeMusic walkMusic;
	
	Color originalColor;
	Color invisibleColor;
	
	// Use this for initialization
	void Start () {
		spawnPoint = transform.position;
		hasMovedRight = false;
		displayedLeftHelp = false;
		displayedRightHelp = true;
		invisibleColor = new Color(0, 0, 0, 0);
		originalColor = sprite.GetComponent<Renderer>().material.GetColor("_TintColor");
	}
	
	public void Respawn() {
		//gameObject.SetActiveRecursively(false);
		transform.position = spawnPoint;
		//gameObject.SetActiveRecursively(true);
		status = PlayerStatus.playing;
		searchedDesk = false;
		mySecretPlace = null;
		spriteAnimation.SetAnimation("idle");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (status == PlayerStatus.busted) {
			if (!sprite.GetComponent<Renderer>().material.GetColor("_TintColor").Equals(originalColor))
				sprite.GetComponent<Renderer>().material.SetColor("_TintColor", originalColor);
			if (Input.GetKeyDown(KeyCode.Space)) {
				Respawn();
			}
			if (walkMusic)
				walkMusic.Disable();
			return;	
		}
		if (status == PlayerStatus.arrived) {
			if (walkMusic)
				walkMusic.Disable();
			return;
		}
		if (status == PlayerStatus.hidden || status == PlayerStatus.onCopier) {
			if (!sprite.GetComponent<Renderer>().material.GetColor("_TintColor").Equals(invisibleColor))
				sprite.GetComponent<Renderer>().material.SetColor("_TintColor", invisibleColor);
			return;
		}
		
		if (!sprite.GetComponent<Renderer>().material.GetColor("_TintColor").Equals(originalColor))
			sprite.GetComponent<Renderer>().material.SetColor("_TintColor", originalColor);
		
		bool left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A);
		bool right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

		if (left && !hasMovedLeft)
			hasMovedLeft = true;
		if (right && !hasMovedRight)
			hasMovedRight = true;
		
		
		horizontal = (left ? -1 : right ? 1 : 0);
		
		bool move = false;
		
		if (horizontal != 0) {
			Vector3 scale = transform.localScale;
			scale.x = Mathf.Abs(scale.x) * horizontal;
			transform.localScale = scale;

			Debug.DrawRay(transform.position + offset, Vector3.right * horizontal * rayLength, Color.green, 1f);
			bool hit = false;
			if (Physics.Raycast (transform.position + offset, Vector3.right * horizontal, out hitInfo, rayLength, layerMask.value))  {
				if (!hitInfo.collider.isTrigger)
					hit = true;
			}
			if (!hit)
				move = true;
		}
		
		if (move) {
			spriteAnimation.SetAnimation("walk");
			transform.Translate(Vector3.right * horizontal * speed * Time.deltaTime);
			if (walkMusic)
				walkMusic.Enable();
		} else {
			spriteAnimation.SetAnimation("idle");
			if (walkMusic)
				walkMusic.Disable();
		}
	}
	
	void OnGUI() {
		if (status == PlayerStatus.busted) {
			GUI.Label(new Rect(Screen.width/2 - 200, Screen.height - 80, 400, 60), "Busted ! Press [space] to restart");
		}
	}
	
}
