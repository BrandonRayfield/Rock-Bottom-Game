using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart_UI : MonoBehaviour {

    private int currentLevel;
    private int mainMenu = 0;

	// Use this for initialization
	void Start () {
        currentLevel = SceneManager.GetActiveScene().buildIndex;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void restartCurrentLevel() {
        SceneManager.LoadScene(currentLevel);
    }

    public void returnToMainMenu() {
        SceneManager.LoadScene(mainMenu);
    }
}
