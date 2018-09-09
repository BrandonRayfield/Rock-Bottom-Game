using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Manager : MonoBehaviour {



    public GameObject[] targets;
    public bool isQuest, isNPCQuest;
    public int questID;
    private int targetsLeft;
    private int totalTargets;

    public string animationName = "doorOpening";

    public GameObject lockedObject;
    private Animator lockedObjectAnimator;
    private GameObject cameraObject;

    private List<bool> isTriggered;
    private bool hasUpdated;
    private bool isComplete, newFocus;

	// Use this for initialization
	void Start () {
        lockedObjectAnimator = lockedObject.GetComponent<Animator>();

        // Get total amount of targets
        totalTargets = targets.Length;

        isTriggered = new List<bool>();

        try {
            cameraObject = GameObject.Find("Camera");
        }
        catch {
            cameraObject = null;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(hasUpdated) {

            isTriggered.Clear();

            // Retrieves list of target triggered status's
            foreach (GameObject target in targets) {
                isTriggered.Add(target.GetComponent<Target>().getIsTriggered());
            }

            targetsLeft = 0;

            // Retrieves list of target triggered status's
            foreach (bool trigger in isTriggered) {
                if(!trigger) {
                    targetsLeft++;
                    //isComplete = false;
                    //break;
                } //else {

                //isComplete = true;
                //newFocus = true;
                //}
            }

            if (isQuest) {
                GameManager.instance.SetCounter(questID, totalTargets - targetsLeft);
            }

            if (targetsLeft == 0) {
                isComplete = true;
                newFocus = true;
            }

            hasUpdated = false;
        }

        if(!isNPCQuest && isComplete) {
            GameManager.instance.CompletedQuest(questID);
            lockedObjectAnimator.Play(animationName);
            if (newFocus) {
                cameraObject.GetComponent<CameraScript>().SetFocusPoint(lockedObject);
                newFocus = false;
            }
        }		
	}

    public void setHasUpdated (bool updated) {
        hasUpdated = updated;
    }

    public int getQuestID() {
        return questID;
    }
}
