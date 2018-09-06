using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelExit : MonoBehaviour {

    //Time variables
    private float time;
    public float restartTime = 4.0f;

    //Play object
    public GameObject playerObject;

    //Text variables
    private bool canFinish = false;
    private bool isFinished = false;
    public Text interactText;
    public Text gameResult;

    // Fade Variables
    public Image fadeImage;

    public int sceneNumber;

    void Start() {
        try {
            playerObject = GameObject.Find("Player");
            interactText = GameObject.Find("InteractText").GetComponent<Text>();
            gameResult = GameObject.Find("ResultText").GetComponent<Text>();
        } catch {
            playerObject = null;
        }
    }

    // Update is called once per frame
    void Update() {

        if (isFinished) {
            time += Time.deltaTime;

            if (time > restartTime) {
                fadeImage.GetComponent<Fade_Script>().LoadNewLevel(sceneNumber);
            }
        }

        if (canFinish && Input.GetKeyDown("e")) {
            interactText.enabled = false;
            gameResult.text = "Level Complete!";
            gameResult.enabled = true;
            isFinished = true;
        }
    } 

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            interactText.text = "Press 'E' to continue!";
            interactText.enabled = true;
            canFinish = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerObject) {
            interactText.text = "";
            interactText.enabled = false;
            canFinish = false;
        }
    }
}
