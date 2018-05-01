using UnityEngine;
using System.Collections;

public class JumpOnCopier : MonoBehaviour {
	
	public GameObject copierSprite;
	public SimpleSpriteAnimation copierSpriteAnimation;
	
	public float flickeringMin = 0.2f;
	public float flickeringMax = 3f;
	public float onDuration = 0.25f;
	bool flickered = false;
	public Color flickeringOn = Color.cyan;
	public Color flickeringOff = Color.white;
	
	bool switched = false;
	public bool hidden = false;
	bool playerInsideTrigger = false;
	float nextFlick;
	
	// Use this for initialization
	void Start () {
		flickered = false;
		nextFlick = Time.time + Random.Range(flickeringMin, flickeringMax);
		switched = false;
		SetCopierStatus(false);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player near copier");
			playerInsideTrigger = true;
			//Show Help - Up Arrows
			ActiveState.SetActive(GlobalHandler.Instance.upArrow, true);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player not near copier anymore");
			playerInsideTrigger = false;
		}
	}
	
	void PlayerOutside(bool force) {
		if (SneakerPlayer.mySecretPlace == gameObject || force) {
			ActiveState.SetActive(GlobalHandler.Instance.upArrow, false);
			// Reset copier display
			copierSpriteAnimation.SetAnimation("idle");
			hidden = false;
			SneakerPlayer.mySecretPlace = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GlobalHandler.Instance.player.status == SneakerPlayer.PlayerStatus.busted) {
			playerInsideTrigger = false;
			PlayerOutside(true);
			return;
		}
		if (Time.time > nextFlick) {
			flickered = !flickered;
			if (flickered) {
				nextFlick = Time.time + onDuration;
				//copierSprite.color = flickeringOn;
			} else {
				nextFlick = Time.time + Random.Range(flickeringMin, flickeringMax);
				//copierSprite.color = flickeringOff;
			}
		}
		if (!playerInsideTrigger)
			return;
		
		bool up = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W);
		
		if (up && !switched) {
			switched = true;
			SetCopierStatus(!hidden);
		} else if (switched) {
			switched = false;
		}
	}
	
	void SetCopierStatus(bool playerHidden) {
		if (playerHidden) {
			if (SneakerPlayer.mySecretPlace != null)
				return;
			hidden = playerHidden;
			// Player sprite is invisible (integrated in copier) but is still visible from guards ! (mouhahahah)
			SneakerPlayer.mySecretPlace = gameObject;
			copierSpriteAnimation.SetAnimation("busy");
			GlobalHandler.Instance.player.status = SneakerPlayer.PlayerStatus.onCopier;
		} else {
			PlayerOutside(false);
			GlobalHandler.Instance.player.status = SneakerPlayer.PlayerStatus.playing;
		}
	}
}
