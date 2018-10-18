using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour {

    public GameObject creditObject;
    public Animator creditAnimator;
    public GameObject skipButton;
    public GameObject slowButton;

    private void Start() {
        skipButton.SetActive(false);
        slowButton.SetActive(false);
    }

    public void SetupCredits() {
        creditAnimator.Play("CreditSetup");
    }

    public void RollCredits() {
        skipButton.SetActive(true);
        slowButton.SetActive(true);
        creditAnimator.Play("CreditRoll");
    }

    public void FinishCredits() {
        creditAnimator.speed = 1;
        skipButton.SetActive(false);
        slowButton.SetActive(false);
        Invoke("PlayFinishAnimation", 3);
    }

    public void PlayFinishAnimation() {
        creditAnimator.Play("BackToMenu");
    }

    public void SkipCredits() {
        creditAnimator.speed += 1;
    }

    public void SlowCredits() {
        creditAnimator.speed -= 1;
    }
}
