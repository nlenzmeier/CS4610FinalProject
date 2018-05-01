using UnityEngine;
using System.Collections;

public class OpenCar : MonoBehaviour {
	
	public SimpleSpriteAnimation carSpriteAnimation;
	public AudioClip openDoorAudioClip;
	public AudioClip closeDoorAudioClip;
	
	bool switched = false;
	public bool opened = false;
	bool playerInsideTrigger = false;
	//int tryCount;
	
	// Use this for initialization
	void Start () {
		//tryCount = 0;
		switched = false;
		SetCarStatus();
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player near car");
			playerInsideTrigger = true;
			//Show Up Arrow
			ActiveState.SetActive(GlobalHandler.Instance.upArrow, true);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player not near car anymore");
			playerInsideTrigger = false;
			ActiveState.SetActive(GlobalHandler.Instance.upArrow, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!playerInsideTrigger)
			return;
		// Does nothing more when player is at the end
		if (GlobalHandler.Instance.player.status == SneakerPlayer.PlayerStatus.arrived)
			return;
		
		bool up = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W);
		
		if (up && ! switched) {
			Debug.Log("Switch on");
			switched = true;
			opened = !opened;
			SetCarStatus();
		} else if (switched) {
			Debug.Log("Switch off " + opened);
			switched = false;
		}
	}
	
	void SetCarStatus() {
		if (opened) {
			carSpriteAnimation.SetAnimation("opened");
			GlobalHandler.Instance.fxSource.PlayOneShot(openDoorAudioClip);
		} else {
			carSpriteAnimation.SetAnimation("closed");
			GlobalHandler.Instance.fxSource.PlayOneShot(openDoorAudioClip);
		}
	}
}
