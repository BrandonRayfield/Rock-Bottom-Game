using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    // Player Object
    public GameObject playerObject;

    // Booleans
    public bool GameIsPaused;
    private bool weaponUIOpen;
    public bool controlUIOpen;
    
    // UI Objects
    public GameObject pauseMenuUI;
    public GameObject controlMenuUI;
    public GameObject weaponSelectUI;

    // Weapon Objects
    public GameObject guitarObject;
    public GameObject harmonicaObject;

    private void Start() {
        GameIsPaused = false;
        weaponUIOpen = false;
        weaponSelectUI.SetActive(false);

        try {
            playerObject = GameObject.Find("Player");
        } catch {
            playerObject = null;
        }

    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("3")) {
            selectHarmonica();
        }

        if (!GameIsPaused && !weaponUIOpen && Input.GetKeyDown(KeyCode.Q)) {
            weaponUIOpen = true;
            Time.timeScale = 0.25f;
            weaponSelectUI.SetActive(true);
            playerObject.GetComponent<Player>().setCanMove(false);
            guitarObject.GetComponent<Weapon>().setCanAttack(false);
            //weaponSelectUI.GetComponent<Animator>().Play("Expand");
        } else if (!GameIsPaused && weaponUIOpen && Input.GetKeyDown(KeyCode.Q)) {
            weaponUIOpen = false;
            Time.timeScale = 1f;
            weaponSelectUI.SetActive(false);
            playerObject.GetComponent<Player>().setCanMove(true);
            guitarObject.GetComponent<Weapon>().setCanAttack(true);
            //weaponSelectUI.GetComponent<Animator>().Play("Close");
        }

        if (!controlUIOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } 
            else
            {
                Pause();
            }
        }

        if (controlUIOpen && Input.GetKeyDown(KeyCode.Escape)) {
            BackButton();
        }
	}

    public void selectGuitar() {
        Debug.Log("Selected Guitar");
        guitarObject.SetActive(true);
        harmonicaObject.SetActive(false);

        weaponUIOpen = false;
        Time.timeScale = 1f;
        weaponSelectUI.SetActive(false);
        playerObject.GetComponent<Player>().setCanMove(true);
        guitarObject.GetComponent<Weapon>().setCanAttack(true);
    }

    public void selectHarmonica() {
        Debug.Log("Selected Harmonica");
        harmonicaObject.SetActive(true);
        guitarObject.SetActive(false);

        weaponUIOpen = false;
        Time.timeScale = 1f;
        weaponSelectUI.SetActive(false);
        playerObject.GetComponent<Player>().setCanMove(true);
        guitarObject.GetComponent<Weapon>().setCanAttack(true);
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        playerObject.GetComponent<Player>().setCanMove(true);
        guitarObject.GetComponent<Weapon>().setCanAttack(true);
    }

    public void Pause ()
    {
        if (!GameIsPaused) {
            weaponUIOpen = false;
            weaponSelectUI.SetActive(false);

            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
            playerObject.GetComponent<Player>().setCanMove(false);
            guitarObject.GetComponent<Weapon>().setCanAttack(false);

        } else {
            Resume();
        }

    }

    public void LoadControls ()
    {
        controlUIOpen = true;
        pauseMenuUI.SetActive(false);
        controlMenuUI.SetActive(true);
    }

    public void BackButton()
    {
        controlUIOpen = false;
        pauseMenuUI.SetActive(true);
        controlMenuUI.SetActive(false);
    }

    public void LoadMenu ()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame ()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
