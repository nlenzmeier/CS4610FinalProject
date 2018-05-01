using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {
	
	public enum DoorStatus {opened, opening, closed, closing};
	public DoorStatus currentStatus = DoorStatus.closed;
	public float closeDelay = 2.0f;
	public Vector3 movement;
	public float positionThreshold = 0.01f;
	
	private float speed = 1;
	private float objectDetectedTime = 0;
	private Vector3 closedPosition;
	private Vector3 openedPosition;
	bool ready = false;
	
	private Vector3 targetPosition;
	private DoorStatus targetStatus;
	
	private AudioSource mySound;
	
	void Awake() {
		ready = false;
	}
	
	void Start() {
		closedPosition = transform.position;
		openedPosition = closedPosition + movement;
		speed = movement.magnitude / closeDelay;
		mySound = GetComponent<AudioSource>();
		ready = true;
	}
	
	// Use this for initialization
	void OnTriggerEnter () {
		if (!ready)
			return;
		objectDetectedTime = Time.time;
		Open();
	}
	
	// Update is called once per frame
	void OnTriggerStay () {
		if (!ready)
			return;
		objectDetectedTime = Time.time;
		Open();
	}
	
	void Update() {
		switch (currentStatus) { 
			case DoorStatus.closing :
			case DoorStatus.opening: Moving(); break;
			case DoorStatus.opened: CheckAutoClose(); break;
		}
	}
	
	private void CheckAutoClose() {
		// Auto close when nothing is detected after closing delay
		if (objectDetectedTime + closeDelay < Time.time) {
			Close();
		}
	}
	
	private void Moving() { 
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
		if ((transform.position - targetPosition).sqrMagnitude < positionThreshold) {
			transform.position = targetPosition;
			currentStatus = targetStatus;
		}
	}
	
	public void Open() {
		if (currentStatus == DoorStatus.closed || currentStatus == DoorStatus.closing) {
			currentStatus = DoorStatus.opening;
			targetPosition = openedPosition;
			targetStatus = DoorStatus.opened;
			mySound.Play();
		}
	}
	
	public void Close() {
		if (currentStatus == DoorStatus.opened || currentStatus == DoorStatus.opening) {
			currentStatus = DoorStatus.closing;
			targetPosition = closedPosition;
			targetStatus = DoorStatus.closed;
			mySound.Play();
		}
	}
}
