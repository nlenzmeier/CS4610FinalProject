using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNatureAudio : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("nature_music");
        for (int i = 0; i < objs.Length; i++)
            Destroy(objs[i]);        
    }
}
