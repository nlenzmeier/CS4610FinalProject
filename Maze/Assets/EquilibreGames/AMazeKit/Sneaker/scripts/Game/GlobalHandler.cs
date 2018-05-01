using UnityEngine;
using System.Collections;

public class GlobalHandler : MonoBehaviour {
	
	public SneakerPlayer player;
	public Renderer playerRenderer;
	
	public GameObject help;
	public GameObject leftArrow;
	public GameObject rightArrow;
	public GameObject upArrow;
	public GameObject downArrow;
	
	public AudioSource fxSource;
	
	static GlobalHandler _instance;
	
	public static GlobalHandler Instance { get { return _instance; } }
	
	// Use this for initialization
	void Awake () {
		_instance = this;
		GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
		if (playerGO != null)
			player = playerGO.GetComponent<SneakerPlayer>();
	}
}
