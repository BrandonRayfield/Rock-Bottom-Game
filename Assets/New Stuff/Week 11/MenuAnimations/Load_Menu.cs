using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load_Menu : MonoBehaviour {

    public Image imageUIObject;
    public Text titleText;

    public Comic[] comicArray;
    private List<Comic> comicList;

    private int nextIndex;

    private Comic nextScene;

    private bool isTalking;
    private bool finishedTalking;

    public bool isEnding;
    public GameObject fadeImage;
    public GameObject endScreen;

    // Time Variables
    private float currentTime;
    private float maxTime;

    // Sound Objects
    public GameObject errorSound;
    private GameObject currentSound;
    private bool triggerSound;

    public Animator animator;
    public GameObject menuObjects;

    // Use this for initialization
    void Start() {

        finishedTalking = true;

        comicList = new List<Comic>();

        StartDialogue(comicArray);

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            previousSlide();
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            nextSlide();
        }

        // Trigger sound if required
        if (triggerSound) {
            currentTime += Time.deltaTime;
            if (currentTime >= maxTime) {
                Instantiate(currentSound, transform.position, transform.rotation);
                triggerSound = false;
            }
        }
    }

    public void nextSlide() {
        Debug.Log(nextIndex);
        if (nextIndex < comicList.Count - 1) {
            if (finishedTalking) {
                nextIndex++;
            }
            DisplayNextSentence();
        } else {
            if (!finishedTalking) {
                DisplayNextSentence();
            } else {
                if (!isEnding) {
                    Instantiate(errorSound, transform.position, transform.rotation);
                } else {
                    Instantiate(errorSound, transform.position, transform.rotation);
                }

            }
        }
    }

    public void previousSlide() {
        Debug.Log(nextIndex);
        if (nextIndex > 0) {
            if (finishedTalking) {
                nextIndex--;
            }
            DisplayNextSentence();
        } else {
            if (!finishedTalking) {
                DisplayNextSentence();
            } else {
                BackToMenu();
            }
        }
    }

    public void StartDialogue(Comic[] comicArray) {

        isTalking = true;

        nextIndex = 0;

        comicList.Clear();

        foreach (Comic scene in comicArray) {
            comicList.Add(scene);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        nextScene = comicList[nextIndex];
        imageUIObject.sprite = nextScene.comicImage;
        titleText.text = nextScene.pageName;

        currentSound = nextScene.sound;
        maxTime = nextScene.soundDelay;
        triggerSound = false;

        // Trigger sound if required
        if (nextScene.sound != null) {
            triggerSound = true;
            currentTime = 0;
        }
    }

    public void BackToMenu() {
        menuObjects.SetActive(true);
        animator.Play("Move_Right");
    }

    public void LoadNextScene() {
        SceneManager.LoadScene(nextIndex + 1);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
