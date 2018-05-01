using UnityEngine;
using System.Collections;

public class ClimbLadder : MonoBehaviour {
	
	public float speed = 1;
	public float rayLength = 2f;
	public int offsetX = 64;
	
	public AudioClip ladderStep;
	
	Transform playerT;
	Rigidbody playerRb;
	bool playerInsideTrigger = false;
	
	// Use this for initialization
	void Start () {
		GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
		if (playerGO != null) {
			playerT = playerGO.transform;
			playerRb = playerGO.GetComponent<Rigidbody>();
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player near ladder");
			playerInsideTrigger = true;
			playerRb.useGravity = false;
			//player.mySecretPlace = gameObject;
			//Show Up and Down Arrows
			//GlobalHandler.Instance.help.transform.position = GlobalHandler.Instance.player.transform.position;
			ActiveState.SetActive(GlobalHandler.Instance.downArrow, true);
			ActiveState.SetActive(GlobalHandler.Instance.upArrow, true);
			// Sound set
			if (GlobalHandler.Instance.fxSource.isPlaying)
				GlobalHandler.Instance.fxSource.Stop();
			GlobalHandler.Instance.fxSource.loop = true;
			GlobalHandler.Instance.fxSource.clip = ladderStep;
		}
	}
	
	void OnTriggerStay(Collider other) {
		if (other.CompareTag("Player")) {
			//Debug.Log("Player near ladder");
			playerInsideTrigger = true;
			playerRb.useGravity = false;
			// Move Up and Down Arrows
			//GlobalHandler.Instance.help.transform.position = GlobalHandler.Instance.player.transform.position;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player not near ladder anymore");
			PlayerOutside();
		}
	}
	
	void PlayerOutside() {
		playerInsideTrigger = false;
		playerRb.useGravity = true;
		//player.mySecretPlace = null;
		ActiveState.SetActive(GlobalHandler.Instance.upArrow, false);
		ActiveState.SetActive(GlobalHandler.Instance.downArrow, false);
		// Sound set
		if (GlobalHandler.Instance.fxSource.isPlaying)
			GlobalHandler.Instance.fxSource.Stop();
		GlobalHandler.Instance.fxSource.loop = false;
			// */
	}
	
	// Update is called once per frame
	void Update () {
		if (GlobalHandler.Instance.player.status == SneakerPlayer.PlayerStatus.busted) {
			PlayerOutside();
			return;
		}
		if (GlobalHandler.Instance.player.status == SneakerPlayer.PlayerStatus.hidden || GlobalHandler.Instance.player.status == SneakerPlayer.PlayerStatus.onCopier)
			return;
		
		if (!playerInsideTrigger)
			return;
		
		bool up = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W);
		bool down = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
		
		if (up) {
			playerT.Translate(Vector3.up * speed * Time.deltaTime);
			// Center player position on the ladder position
			Vector3 pos = playerT.transform.position;
			pos.x = transform.position.x + offsetX;
			playerT.transform.position = pos;
			
			if (!GlobalHandler.Instance.fxSource.isPlaying)
				GlobalHandler.Instance.fxSource.Play();
		}
		else if (down) {
			Debug.DrawRay(playerT.position, Vector3.down * rayLength, Color.cyan, 0.5f);
			// Prevent going through the floor while on the ladder
			if (!Physics.Raycast (playerT.position, Vector3.down, rayLength, GlobalHandler.Instance.player.layerMask.value))  {
				playerT.Translate(Vector3.down * speed * Time.deltaTime);
				
				// Center player position on the ladder position
				Vector3 pos = playerT.transform.position;
				pos.x = transform.position.x + offsetX;
				playerT.transform.position = pos;
				
				if (!GlobalHandler.Instance.fxSource.isPlaying)
					GlobalHandler.Instance.fxSource.Play();
			} else {
				GlobalHandler.Instance.fxSource.Stop();
			}
		} else {
			GlobalHandler.Instance.fxSource.Stop();
		}
	}
}
