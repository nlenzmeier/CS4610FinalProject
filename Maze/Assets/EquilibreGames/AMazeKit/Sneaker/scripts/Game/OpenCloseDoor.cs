using UnityEngine;
using System.Collections;

public class OpenCloseDoor : MonoBehaviour {
	
	public SimpleSpriteAnimation doorSprite;
	public Collider doorCollider;
	
	public AudioClip openDoorAudioClip;
	public AudioClip closeDoorAudioClip;
	
	bool switched = false;
	public bool opened = false;
	bool playerInsideTrigger = false;
	
	void Awake() {
		if (doorSprite == null)
			doorSprite = GetComponent<SimpleSpriteAnimation>();
	}
	
	// Use this for initialization
	void Start () {
		switched = false;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			playerInsideTrigger = true;
			//Show Up Arrow
			//GlobalHandler.Instance.help.transform.position = GlobalHandler.Instance.player.transform.position;
			//GlobalHandler.Instance.upArrow.SetActiveRecursively(true);
		} else if (other.GetComponent<Guard>()!=null) {
			SetDoorStatus(true);
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.GetComponent<Guard>()!=null) {
			SetDoorStatus(true);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			playerInsideTrigger = false;
			//GlobalHandler.Instance.upArrow.SetActiveRecursively(false);
		} else if (other.GetComponent<Guard>()!=null) {
			SetDoorStatus(false);
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
			// Making the sound here makes it sound only for player, not guards
			if (opened) {
				GlobalHandler.Instance.fxSource.PlayOneShot(closeDoorAudioClip);
			} else {
				GlobalHandler.Instance.fxSource.PlayOneShot(openDoorAudioClip);
			}
			
			SetDoorStatus(!opened);
		} else if (switched) {
			Debug.Log("Switch off " + opened);
			switched = false;
		}
	}
	
	void SetDoorStatus(bool newStatus) {
		if (newStatus == opened)
			return;
		
		opened = newStatus;
		if (opened) {
			doorSprite.SetAnimation("opened");
			doorCollider.enabled = false;
		} else {
			doorSprite.SetAnimation("closed");
			doorCollider.enabled = true;
		}
	}
}
