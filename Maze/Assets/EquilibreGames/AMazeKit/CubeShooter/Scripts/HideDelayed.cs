using UnityEngine;
using System.Collections;

public class HideDelayed : MonoBehaviour {
	
	enum status {shown, hidden};
	private status currentStatus;
	public GameObject shadow;
	public float delay = 5f;
	private float hiddenTime;
	public Color enabledShadowColor = new Color(0, 0, 0, 0.5f);
	public Color enabledTextColor = new Color(1, 1, 1, 1f);
	private Color disabledColor = new Color(0, 0, 0, 0);
	
	// Use this for initialization
	void Awake () {
		Hide ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentStatus == status.shown && Time.time > hiddenTime) {
			Hide ();
		}
	}
	
	public void Show() {
		hiddenTime = Time.time + delay;
		//Debug.Log(Time.time + " > Show " + gameObject.name + " until " + hiddenTime);
		currentStatus = status.shown;
		shadow.GetComponent<Renderer>().sharedMaterial.color = enabledShadowColor;
		GetComponent<GUIText>().material.color = enabledTextColor;
	}
	
	public void Hide() {
		hiddenTime = Time.time-1;
		currentStatus = status.hidden;
		shadow.GetComponent<Renderer>().sharedMaterial.color = disabledColor;
		GetComponent<GUIText>().material.color = disabledColor;
	}
}
