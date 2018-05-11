using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour {

    public Transform PauseCanvas;
    public Transform DirCanvas;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
       
	}

    public void Pause()
    {
        if (PauseCanvas.gameObject.activeInHierarchy == false)
        {
            PauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PauseCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }


    public void Directions()
    {
        DirCanvas.gameObject.SetActive(true);

    }

    public void UnDirections()
    {
        // undoes directions
        DirCanvas.gameObject.SetActive(false);
    }

    public void MainMenu()
    {
	Time.timeScale = 1;
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
    
    
}
