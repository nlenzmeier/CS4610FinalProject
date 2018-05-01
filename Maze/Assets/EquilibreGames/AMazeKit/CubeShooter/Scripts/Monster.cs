using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
	
	public enum status {idle, attacking, repulsed, returningToBase};
	
	public status currentStatus = status.idle;
	public int health = 5;
	public int xpGain = 10;
	
	protected GameObject player;
	private Renderer myRenderer;
	private bool bulletEnabled = false;
	private float bulletDelay = 0.5f;
	private float bulletSpeed = 0.5f;
	private float nextBullet;
	public float nearMagnitude = 5;
	public float speed = 5;
	protected Vector3 initialPosition;
	public float maxVelocity = 5;
	protected float goBackToNormalTime;
	public float repulsedTime = 2;
	protected AudioSource mySource;
	public AudioSource myBulletSource;
	public GameObject bulletPrefab;
	public SphereCollider triggerSphere;
	public bool fightBack = false;
	public Material disabledSkin;
	protected bool sideWalk;
	float directionChangeDelay;
	float sideSpeedChangeDelay;
	float nextSideSpeedChange;
	float sideSpeed;
	float nextDirectionChange;
	Vector3 direction;
	
	public AudioClip hurtPlayer;
	public AudioClip disabled;
	
	[System.Serializable]
	public class Level {
		public float directionChangeDelay = 1f;
		public bool bullet = false;
		public float bulletDelay = 0.4f;
		public float bulletSpeed = 20;
		public int health = 20;
		public int speed = 5;
		public int nearMagnitude = 300;
		public int xpGain = 10;
		public float repulsedTime = 1;
		public bool fightBack = false;
		public Material skin;
		public bool sideWalk = false;
		public float sideSpeed = 1;
		public float sideSpeedChangeDelay = 0.25f;
	}
	
	public Level[] levels;
	
	private int currentLevelIndex;
	private Level currentLevel;
	
	// Use this for initialization
	public virtual void Start () {
		player = GameObject.FindGameObjectWithTag("Player");	
		triggerSphere = GetComponent<SphereCollider>();
		initialPosition = transform.position;
		mySource = GetComponent<AudioSource>();
		myRenderer = GetComponent<Renderer>();
		currentLevelIndex = 0;
		
		if (player == null || triggerSphere == null)
			ActiveState.SetActive(gameObject, false);
		
		UpdateLevelMonsterData();
	}
	
	public void OnBecameVisible() {
		if (health <= 0)
			return;
		enabled = true;
		GetComponent<Rigidbody>().isKinematic = false;
	}
	public void OnBecameInvisible() {
		if (health <= 0)
			return;
		enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
	}
	
	void UpdateLevelMonsterData() {
		currentLevel = levels[currentLevelIndex];
		directionChangeDelay = currentLevel.directionChangeDelay;
		health = currentLevel.health;
		bulletDelay = currentLevel.bulletDelay;
		bulletSpeed = currentLevel.bulletSpeed;
		bulletEnabled = currentLevel.bullet;
		speed = currentLevel.speed;
		triggerSphere.radius = currentLevel.nearMagnitude;
		xpGain = currentLevel.xpGain;
		repulsedTime = currentLevel.repulsedTime;
		myRenderer.material = currentLevel.skin;
		fightBack = currentLevel.fightBack;
		sideSpeed = currentLevel.sideSpeed;
		sideWalk = currentLevel.sideWalk;
		sideSpeedChangeDelay = currentLevel.sideSpeedChangeDelay;
		
		nextDirectionChange = Time.time;
		nextSideSpeedChange = Time.time + sideSpeedChangeDelay;
		direction = Vector3.zero;
	}

	void CheckLevel ()
	{
		if (currentLevelIndex != CSPlayer.currentLevelIndex && currentLevelIndex < levels.Length-1) {
			currentLevelIndex = CSPlayer.currentLevelIndex;
			UpdateLevelMonsterData();
		}
	}
	
	void OnTriggerEnter(Collider collider) {
		if (health <= 0)
			return;
		if (collider.CompareTag("Player")) {
			currentStatus = status.attacking;
			// Upgrade at the same time the player does, if available
			CheckLevel ();
		}
	}
	
	void OnTriggerExit(Collider collider) {
		if (health <= 0)
			return;
		if (collider.CompareTag("Player")) {
			currentStatus = status.returningToBase;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentStatus) {
			case status.repulsed : UpdateRepulsed(); break;
			case status.attacking : UpdateAttacking(); break;
			case status.returningToBase: UpdateReturnToBase(); break;
		}
	}
	
	void UpdateRepulsed() {
			if (Time.time < goBackToNormalTime)
				return;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			currentStatus = status.attacking;
		}
	
	void UpdateAttacking() {
		Vector3 lookDirection = (player.transform.position - transform.position).normalized;
		if (Time.time > nextDirectionChange) {
			nextDirectionChange = Time.time + directionChangeDelay;
			direction = lookDirection;
			bool applySpeed = true;
			if (sideWalk && Time.time > nextSideSpeedChange) {
				// a little sidewalk ?
				int side = Random.Range(0, 100);
				if (side < 50) {
					direction = Vector3.Cross(Vector3.up, direction) * -sideSpeed;
					applySpeed = false;
				} else {
					direction = Vector3.Cross(Vector3.up, direction) * sideSpeed;
					applySpeed = false;
				}
				nextSideSpeedChange = Time.time + sideSpeedChangeDelay;
			}
			if (applySpeed)
				direction *= speed;
		}
		
		transform.Translate(direction * Time.deltaTime, Space.World);
		if (direction.sqrMagnitude > 0.1f) {
			// Update look-at rotation
			lookDirection.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
			transform.rotation = rotation;
		}
		
		if (bulletEnabled && Time.time > nextBullet) {
			// instantiate bullet
			GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);
			// set velocity
			bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed + direction;
			nextBullet = Time.time + bulletDelay;
			myBulletSource.Play();
		}
	}
	
	void UpdateReturnToBase() {
		if ((initialPosition - transform.position).sqrMagnitude < 5) {
			currentStatus = status.idle;
		}
		Vector3 move = (initialPosition - transform.position).normalized;
		transform.Translate(move * speed * Time.deltaTime, Space.World);
		if (move.sqrMagnitude > 0.1f) {
			// Update look-at rotation
			Vector3 direction = new Vector3(move.x, 0, move.z);
			Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
			transform.rotation = rotation;
		}
	}
	
	void FixedUpdate() {
		if (!GetComponent<Rigidbody>().isKinematic){
    		GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxVelocity);
  		}
	}
	
	void Hurt() {
		if (health <= 0)
			return;
		health--;
		mySource.Play();
		// Upgrade at the same time the player does, if available
		CheckLevel ();

		if (health <= 0) {
			enabled = false;
			mySource.PlayOneShot(disabled);
			GetComponent<Rigidbody>().useGravity = false;
			//GetComponent<BoxCollider>().enabled = false;
			myRenderer.material = disabledSkin;
			CSPlayer.killed++;
			CSPlayer.xp += xpGain;
		}
		if (fightBack && currentStatus != status.attacking)
			currentStatus = status.attacking;
	}
	
	/*
	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1, 0, 0, 0.25f);
		Gizmos.DrawSphere(transform.position, Mathf.Sqrt(nearMagnitude));
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + rigidbody.velocity);
	}
	// */
	
	void Repulsed() {
		currentStatus = status.repulsed;
		goBackToNormalTime = Time.time + repulsedTime;
		mySource.PlayOneShot(hurtPlayer);
	}
	
}
