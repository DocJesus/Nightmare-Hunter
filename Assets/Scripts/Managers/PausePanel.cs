using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour {

    public bool GameIsPaused;
    public GameObject pausePanel;
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("JoystickStart"))
        {
            if (GameIsPaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("SUpp");
            if (GameIsPaused)
            {
                loadMenu();
            }
        }
	}

    public void pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void loadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
