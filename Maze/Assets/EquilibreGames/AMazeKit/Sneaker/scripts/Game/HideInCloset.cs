using UnityEngine;
using System.Collections;

public class HideInCloset : MonoBehaviour {

	public GameObject closetSprite;
	public SimpleSpriteAnimation closetSpriteAnimation;
	
	public AudioClip hideInAudioClip;
	
	bool switched = false;
	public bool hidden = false;
	bool playerInsideTrigger = false;
	
	// Use this for initialization
	void Start () {
		switched = false;
		SetClosetStatus(false);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			playerInsideTrigger = true;
			//Show Up Arrows
			ActiveState.SetActive(GlobalHandler.Instance.upArrow, true);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player not near closet anymore");
			playerInsideTrigger = false;
			ActiveState.SetActive(GlobalHandler.Instance.upArrow, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!playerInsideTrigger)
			return;
		
		bool up = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W);
		
		if (up && !switched) {
			Debug.Log("Switch on");
			switched = true;
			SetClosetStatus(!hidden);
			GlobalHandler.Instance.fxSource.PlayOneShot(hideInAudioClip);
		} else if (switched) {
			Debug.Log("Switch off " + hidden);
			switched = false;
		}
	}
	
	void SetClosetStatus(bool playerHidden) {
		if (playerHidden) {
			if (SneakerPlayer.mySecretPlace != null)
				return;
			hidden = playerHidden;
			closetSpriteAnimation.SetAnimation("busy");
			SneakerPlayer.mySecretPlace = gameObject;
			GlobalHandler.Instance.player.status = SneakerPlayer.PlayerStatus.hidden;
		} else {
			if (SneakerPlayer.mySecretPlace != gameObject)
				return;
			hidden = playerHidden;
			closetSpriteAnimation.SetAnimation("idle");
			SneakerPlayer.mySecretPlace = null;
			GlobalHandler.Instance.player.status = SneakerPlayer.PlayerStatus.playing;
		}
	}
}
