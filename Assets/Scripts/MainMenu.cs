using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Canvas quitMenu;
    public Canvas menuButtons;

    // Use this for initialization
    void Start () {
        quitMenu = quitMenu.GetComponent<Canvas>();
        menuButtons = menuButtons.GetComponent<Canvas>();

        quitMenu.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitPress() {
        quitMenu.enabled = true;
        menuButtons.enabled = false;
    }

    public void NoPress() {
        quitMenu.enabled = false;
        menuButtons.enabled = true;
    }

    public void LoadGame() {
        SceneManager.LoadScene("Side Scroller");
    }

    public void ExitGame() {
        Application.Quit();
    }

}
