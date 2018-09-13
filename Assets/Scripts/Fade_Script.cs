using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade_Script : MonoBehaviour {

    public Animator animator;
    private int sceneNumber;

    public void FadeToLevel() {
        animator.SetTrigger("FadeOut");
    }

    public void delayedFadeToLevel() {
        animator.SetTrigger("DelayedFadeOut");
        Invoke("OnFadeComplete", 5);
    }

    public void OnFadeComplete() {
        SceneManager.LoadScene(sceneNumber);
    }

    public void LoadNewLevel(int levelIndex, bool isDelay) {
        sceneNumber = levelIndex;

        if(isDelay) {
            delayedFadeToLevel();
        } else {
            FadeToLevel();
        }
    }

}
