using UnityEngine;
using System.Collections;

public class CSPlayer : MonoBehaviour {
	
	public static int health;
	public static int xp = 0;
	public float speed = 1;
	public int repulsiveForce = 5;
	public Camera playerCamera;
	public Vector3 cameraOffset = new Vector3(0, 17.5f, 0);
	public float hurtPause = 0.5f;
	public float nextHurtTime;
	public GameObject shield;
	public float shieldForce = 2f;
	private Texture2D whiteTex;
	private bool shieldActive = false;
	public float shieldRadius;
	public GameObject bulletPrefab;
	public float bulletSpeed = 20;
	public float bulletDelay = 0.2f;
	private float nextBullet;
	public bool shieldEnabled = false;
	public static int killed = 0;
	public Vector3 spawnPosition;
	private int levelStartExperience = 0;
	public LayerMask physicLayers;
	
	public HideDelayed basicInstruction;
	public HideDelayed shieldInstruction;
	
	public AudioSource levelUp;
	
	[System.Serializable]
	public class Level {
		public bool shield = false;
		public float bulletDelay = 0.4f;
		public float bulletSpeed = 20;
		public int health = 20;
		public int xpNeeded = 50;
		public float speed = 10;
	}
	
	public Level[] levels;
	
	public static int currentLevelIndex;
	private Level currentLevel;
	
	private AudioSource mySource;
	
	public float pushPower = 2;
	
	// Use this for initialization
	void Start () {
		mySource = GetComponent<AudioSource>();
		spawnPosition = transform.position;
		
		whiteTex = new Texture2D(1, 4);
		whiteTex.SetPixel(1, 1, Color.grey);
		whiteTex.SetPixel(1, 2, Color.white);
		whiteTex.SetPixel(1, 3, Color.grey);
		whiteTex.SetPixel(1, 4, Color.grey);
		whiteTex.Apply();
		
		shieldActive = false;
#if UNITY_4_0 || UNITY_4_1
		shield.SetActive(shieldActive);
#else
		shield.SetActiveRecursively(shieldActive);
#endif	
		if (playerCamera == null)
			playerCamera = Camera.main;
		
		Init();
		
		if (basicInstruction)
			basicInstruction.Show();
	}
	
	void Init() {
		nextHurtTime = 0;
		nextBullet = 0;
		levelStartExperience = 0;
		transform.position = spawnPosition;
		currentLevelIndex = 0;
		xp = 0;
		UpdateLevelPlayerData();
	}
	
	void UpdateLevelPlayerData() {
		currentLevel = levels[currentLevelIndex];
		health = currentLevel.health;
		if (!shieldEnabled && currentLevel.shield && shieldInstruction)
			shieldInstruction.Show();
		shieldEnabled = currentLevel.shield;
		bulletDelay = currentLevel.bulletDelay;
		speed = currentLevel.speed;
	}
	
	// Update is called once per frame
	void Update () {
		// Level update ?
		if (xp >= currentLevel.xpNeeded) {
			if (currentLevelIndex < levels.Length-1) {
				currentLevelIndex++;
				levelUp.Play();
				levelStartExperience = currentLevel.xpNeeded;
				UpdateLevelPlayerData();
			}
		}
		// Shield (level ?)
		if (shieldEnabled && !shieldActive) {
			shieldActive = true;
#if UNITY_4_0 || UNITY_4_1
		shield.SetActive(shieldActive);
#else
		shield.SetActiveRecursively(shieldActive);
#endif
		} else if (!shieldEnabled && shieldActive) {
			shieldActive = false;
#if UNITY_4_0 || UNITY_4_1
		shield.SetActive(shieldActive);
#else
		shield.SetActiveRecursively(shieldActive);
#endif			
		}
#if UNITY_EDITOR
		if (Input.GetKey(KeyCode.Space) && currentLevelIndex < levels.Length-1) {
				currentLevelIndex++;
			UpdateLevelPlayerData();
		}
#endif
			
		// Bullets (level ?)
		if (Input.GetButton("Fire1") && bulletPrefab != null && Time.time > nextBullet) {
			// instantiate bullet
			GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);
			// set velocity
			bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed + GetComponent<Rigidbody>().velocity;
			nextBullet = Time.time + bulletDelay;
			mySource.Play();
		}
		// Show Instructions ?
		if (Input.mousePosition.y < 40) {
			if (basicInstruction)
				basicInstruction.Show();
			if (shieldEnabled && shieldInstruction)
				shieldInstruction.Show();
		}
		
		// Moves + support for ZQSD / WASD
		float horizontal = Input.GetAxis("Horizontal");
		if (horizontal == 0) {
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
				horizontal = -1;
			else if (Input.GetKey(KeyCode.D))
				horizontal = 1;
		}
		float vertical = Input.GetAxis("Vertical");
		if (vertical == 0) {
			if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
				vertical = 1;
			else if (Input.GetKey(KeyCode.S))
				vertical = -1;
		}
		// Force zero velocity
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		
		Vector3 move = (Vector3.forward * vertical + Vector3.right * horizontal) * speed * Time.deltaTime;
		
		// Move only if there's no obstacle
		Ray ray = new Ray(transform.position, move);
		RaycastHit hit;
		if (!Physics.Raycast(ray, out hit, move.magnitude, physicLayers))
			transform.Translate(move, Space.World);
		// Direction from mouse position
		Vector3 mousePointer = playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCamera.transform.position.y-transform.position.y));
		Vector3 direction = (mousePointer-transform.position).normalized;
		// set at player altitude
		direction.y = 0;
 		if (direction.sqrMagnitude > 0.1f) {
			// Update look-at rotation
			Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
			transform.rotation = rotation;
		}
		
		if (playerCamera != null) {
			// Put camera above the player (do not change Y)
			Vector3 abovePlayer = transform.position;
			abovePlayer += cameraOffset;
			playerCamera.transform.position = abovePlayer;
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.collider.CompareTag("Monster") && Time.time > nextHurtTime) {
			GameObject monsterGO = collision.gameObject;
			Monster monster = monsterGO.GetComponent<Monster>();
			if (monster.enabled) {
				Hurt();
				// Update collider status
				monster.SendMessage("Repulsed");
				// Repulsive force to the attacker (bounce it back)
				Vector3 repulsiveVector = (collision.collider.attachedRigidbody.position - transform.position).normalized * repulsiveForce;
				collision.collider.attachedRigidbody.velocity = repulsiveVector;
			}
		}
	}
	
	void FixedUpdate () {
		if (!shieldActive)
			return;
		
	    Collider[] things = Physics.OverlapSphere(transform.position, shieldRadius);
    	for (int i=0; i<things.Length; i++){
	        if (things[i].attachedRigidbody && things[i].attachedRigidbody != GetComponent<Rigidbody>()){
            	Vector3 offset = (transform.position - things[i].transform.position);
            	things[i].attachedRigidbody.AddForce(offset/offset.magnitude * -shieldForce);
        	}
    	}
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit) {
    Rigidbody body = hit.collider.attachedRigidbody;
    // no rigidbody
    if (body == null || body.isKinematic)
        return;
        
    // We dont want to push objects below us
    if (hit.moveDirection.y < -0.3) 
        return;
    
    // Calculate push direction from move direction, 
    // we only push objects to the sides never up and down
    Vector3 pushDir = new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);

    // If you know how fast your character is trying to move,
    // then you can also multiply the push velocity by that.
    
    // Apply the push
    body.velocity = pushDir * pushPower;
}
	
	void Hurt() {
		health--;
		nextHurtTime = Time.time + hurtPause;
		if (health <= 0) {
			Init();
		}
	}
	
	void OnGUI() {		
		// background
		GUI.color = new Color(0, 0, 0, 0.5f);
		GUI.DrawTexture(new Rect(2, 3, Mathf.Max(282, currentLevel.health*10+82), 44), whiteTex);
		// bar background
		GUI.color = Color.grey;
		GUI.DrawTexture(new Rect(80, 10, Mathf.Min(200, currentLevel.health*10), 10), whiteTex);
		GUI.DrawTexture(new Rect(80, 30, 200, 10), whiteTex);
		
		// Health and XP labels
		GUI.color = Color.white;
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUI.Label(new Rect(4, 4, 70, 18), "Health");
		GUI.Label(new Rect(4, 24, 70, 18), "Level " + (currentLevelIndex+1) + " | XP");
		
		// Health bar
		GUI.color = Color.cyan;
		GUI.DrawTexture(new Rect(81, 11, 10*health-2, 8), whiteTex);

		// XP bar
		GUI.color = Color.magenta;
		int xpProgress = Mathf.Clamp(Mathf.RoundToInt(200f * (xp - levelStartExperience) / (currentLevel.xpNeeded - - levelStartExperience))-2, 2, 198);
		GUI.DrawTexture(new Rect(81, 31, xpProgress, 8), whiteTex);
		
	}

}
