using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Canvas quitMenu;
    public Canvas controlMenu;

    private bool cMenuOpen;
    private bool xMenuOpen;

    // Use this for initialization
    void Start () {
        quitMenu = quitMenu.GetComponent<Canvas>();
        controlMenu = controlMenu.GetComponent<Canvas>();

        cMenuOpen = false;
        xMenuOpen = false;

        quitMenu.enabled = false;
        controlMenu.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitPress() {
        if (!xMenuOpen) {
            quitMenu.enabled = true;
            controlMenu.enabled = false;
            xMenuOpen = true;
            cMenuOpen = false;
        } else {
            quitMenu.enabled = false;
            xMenuOpen = false;
        }

    }

    public void NoPress() {
        quitMenu.enabled = false;
    }

    public void OpenControlMenu() {
        if (!cMenuOpen) {
            quitMenu.enabled = false;
            controlMenu.enabled = true;
            cMenuOpen = true;
            xMenuOpen = false;
        } else {
            controlMenu.enabled = false;
            cMenuOpen = false;
        }

    }

    public void LoadGame() {
        SceneManager.LoadScene(1);
    }

    public void ExitGame() {
        Application.Quit();
    }

}
