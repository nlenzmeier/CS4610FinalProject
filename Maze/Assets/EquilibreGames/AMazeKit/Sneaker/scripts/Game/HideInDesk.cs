using UnityEngine;
using System.Collections;

public class HideInDesk : MonoBehaviour {
	
	public GameObject deskSprite;
	public SimpleSpriteAnimation deskSpriteAnimation;
	
	public static HideInDesk myDesk;
	public float flickeringMin = 0.2f;
	public float flickeringMax = 3f;
	public float onDuration = 0.25f;
	bool flickered = false;
	public Color flickeringOn = Color.cyan;
	public Color flickeringOnTarget = Color.red;
	public Color flickeringOff = Color.white;
	public Color targetDeskColor = Color.cyan;
	
	bool switched = false;
	public bool hidden = false;
	bool playerInsideTrigger = false;
	float nextFlick;
	
	void Awake() {
		if (myDesk == null && Random.Range(1, 100) > 80) {
			myDesk = this;
			deskSprite.GetComponent<Renderer>().material.SetColor("_EmisColor", targetDeskColor);
		}
	}
	
	// Use this for initialization
	void Start () {
		if (myDesk == null)
			myDesk = this;
		
		flickered = false;
		nextFlick = Time.time + Random.Range(flickeringMin, flickeringMax);
		switched = false;
		SetDeskStatus(false);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player near desk");
			playerInsideTrigger = true;
			//Show Up Arrows
			ActiveState.SetActive(GlobalHandler.Instance.upArrow, true);
			ActiveState.SetActive(GlobalHandler.Instance.downArrow, true);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player not near desk anymore");
			playerInsideTrigger = false;
			ActiveState.SetActive(GlobalHandler.Instance.upArrow, false);
			ActiveState.SetActive(GlobalHandler.Instance.downArrow, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextFlick) {
			flickered = !flickered;
			if (flickered) {
				nextFlick = Time.time + onDuration;
				//deskSprite.color = (myDesk == this ? flickeringOnTarget : flickeringOn);
			} else {
				nextFlick = Time.time + Random.Range(flickeringMin, flickeringMax);
				//deskSprite.color = flickeringOff;
			}
		}
		if (!playerInsideTrigger)
			return;
		
		bool up = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W);
		bool down = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
		
		if (down && !switched) {
			Debug.Log("Desk Switch on");
			switched = true;
			SetDeskStatus(!hidden);
		} else if (switched) {
			Debug.Log("Desk Switch off " + hidden);
			switched = false;
		}
		if (up && !GlobalHandler.Instance.player.searchedDesk) {
			if (myDesk == this) {
				// I found it ! End of the game.. phase 1
				GlobalHandler.Instance.player.searchedDesk = true;
			}
		}
	}
	
	void SetDeskStatus(bool playerHidden) {
		if (playerHidden) {
			if (SneakerPlayer.mySecretPlace != null)
				return;
			hidden = playerHidden;
			deskSpriteAnimation.SetAnimation("busy");
			SneakerPlayer.mySecretPlace = gameObject;
			GlobalHandler.Instance.player.status = SneakerPlayer.PlayerStatus.hidden;
		} else {
			if (SneakerPlayer.mySecretPlace != gameObject)
				return;
			hidden = playerHidden;
			deskSpriteAnimation.SetAnimation("free");
			SneakerPlayer.mySecretPlace = null;
			GlobalHandler.Instance.player.status = SneakerPlayer.PlayerStatus.playing;
		}
	}
}
