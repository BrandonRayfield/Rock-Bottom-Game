using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class New_Comic_Controller : MonoBehaviour {

    public Image imageUIObject;
    public Text dialogueText;

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

    // Use this for initialization
    void Start() {

        finishedTalking = true;

        comicList = new List<Comic>();

        StartDialogue(comicArray);

    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
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
            if(!finishedTalking) {
                DisplayNextSentence();
            } else {
                Debug.Log("Loading Next Scene");
                fadeImage.GetComponent<Fade_Script>().LoadNewLevel(0, true);
                Invoke("LoadNextScene", 1);
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
            if(!finishedTalking) {
                DisplayNextSentence();
            } else {
                Debug.Log("Error Sound");
                Instantiate(errorSound, transform.position, transform.rotation);
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

        StopAllCoroutines();

        if (finishedTalking) {
            nextScene = comicList[nextIndex];
            imageUIObject.sprite = nextScene.comicImage;

            currentSound = nextScene.sound;
            maxTime = nextScene.soundDelay;
            triggerSound = false;

            // Trigger sound if required
            if (nextScene.sound != null) {
                triggerSound = true;
                currentTime = 0;
            }

            StartCoroutine(TypeSentence(nextScene.dialogue));
        } else {
            skipTextAnimation(nextScene.dialogue);
        }
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";

        finishedTalking = false;

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.02f);
        }

        finishedTalking = true;

    }

    public void skipTextAnimation(string sentence) {
        dialogueText.text = sentence;
        finishedTalking = true;
    }

    public void LoadNextScene() {
        SceneManager.LoadScene(2);
    }

    public void ExitGame() {
        Application.Quit();
    }

}
