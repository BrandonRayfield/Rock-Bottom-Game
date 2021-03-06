﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Comic_Controller : MonoBehaviour {

    public bool isEnding;
    public GameObject fadeImage;
    public GameObject endScreen;

    public GameObject[] imageObjects;
    public Sprite[] comicPanels;

    public int currentPage;
    public int nextPanel;
    public int currentPanel;
    public int comicLength;


    // Use this for initialization
    void Start () {
        foreach(GameObject image in imageObjects) {
            image.SetActive(false);
        }

        nextPanel = 0;

    }
	
	// Update is called once per frame
	void Update () {

    }

    public void LoadNextImage() {

        comicLength = comicPanels.Length;

        if (nextPanel < comicLength) {

            Debug.Log(nextPanel % imageObjects.Length + 1);

            if (nextPanel % imageObjects.Length + 1 == 1) {
                foreach (GameObject image in imageObjects) {
                    image.SetActive(false);
                }
            }

            currentPanel = nextPanel - imageObjects.Length * currentPage;

            imageObjects[currentPanel].SetActive(true);
            imageObjects[currentPanel].GetComponent<Image>().sprite = comicPanels[nextPanel];
            nextPanel++;

            if (nextPanel % imageObjects.Length == 0) {
                currentPage++;
            }



        } else {
            if(!isEnding) {
                Load_Level();
            } else {
                fadeImage.GetComponent<Fade_Script>().LoadNewLevel(0, true);
                Invoke("showEndScreen", 1);
            }

        }

    }

    public void showEndScreen() {
        endScreen.SetActive(true);
    }

    public void Load_Level() {
        SceneManager.LoadScene(2);
    }

    public void ExitGame() {
        Application.Quit();
    }

}
