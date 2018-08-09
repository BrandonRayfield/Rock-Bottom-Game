using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //Singleton Setup
    public static GameManager instance = null;

    public GameObject[] enemies;
    public Text enemiesRemaining;

	public bool goldKey = false;


    // New Stuff
    public Quest[] questList;
    private GameObject[] questObjects;
    [HideInInspector]
    public List<string> UIQuestText;
    [HideInInspector]
    public string currentUIQuestText;

    private bool questUpdated;

    // Awake Checks - Singleton setup
    void Awake() {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
		foreach (Quest quest in questList) {
            if (quest.isCollectQuest) {
                if(quest.totalAmount == 0) {
                    quest.totalAmount = GetAmountTotal(quest.objectTag);
                    quest.currentAmount = quest.totalAmount - GetAmountTotal(quest.objectTag);
                } else {
                    quest.currentAmount = 0;
                }
                
                quest.previousAmount = quest.currentAmount;
            } else if (quest.isKillQuest) {
                if (quest.totalAmount == 0) {
                    quest.totalAmount = GetAmountTotal(quest.objectTag);
                    quest.currentAmount = quest.totalAmount - GetAmountTotal(quest.objectTag);
                } else {
                    quest.currentAmount = quest.totalAmount - quest.currentAmount;
                }
                
                quest.previousAmount = quest.currentAmount;
            } else if (quest.isDestroyQuest) {
                //quest.targetObject = GameObject.FindGameObjectWithTag(quest.objectTag);
            }
        }

        //Collider setup (bosses move through player)
        Physics.IgnoreLayerCollision(8, 10);
    }
	
	// Update is called once per frame
	void Update () {

        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
		//enemiesRemaining.text = "Enemies Remaining: " + enemies.Length;

        foreach (Quest quest in questList) {

            if(quest.currentAmount >= quest.totalAmount && (quest.isKillQuest || quest.isCollectQuest)) {
                quest.isComplete = true;
            }

            //quest.currentAmount = quest.totalAmount - quest.currentAmount;

            if(quest.isCollectQuest) {
                //quest.currentAmount = quest.totalAmount - GetAmountTotal(quest.objectTag);
            } else if (quest.isKillQuest) {
                quest.currentAmount = quest.totalAmount - GetAmountTotal(quest.objectTag);
            } else if (quest.isDestroyQuest) {
                quest.isComplete = isDestroyed(quest.targetObject, quest);
            }

            if (quest.currentAmount != quest.previousAmount) {
                updateUI();
                quest.previousAmount = quest.currentAmount;
            }

        }

        //enemiesRemaining.text = UIQuestText;


    }

    public int GetAmountTotal(string objectTag) {
        questObjects = GameObject.FindGameObjectsWithTag(objectTag);
        return questObjects.Length;
    }

    public bool GetIsComplete(int IDcheck) {

        if(IDcheck < questList.Length) {
            return questList[IDcheck].isComplete;
        } else {
            throw new Exception("Invalid Quest ID");
        }
    }

    public bool GetHasAccepted(int IDcheck) {

        if (IDcheck < questList.Length) {
            return questList[IDcheck].hasAccepted;
        }
        else {
            throw new Exception("Invalid Quest ID");
        }
    }

    public bool isDestroyed(GameObject target, Quest quest ) {
        if(target == null) {
            quest.currentAmount++;
            return true;
        } else {
            return false;
        }
    }

    public void updateUI() {

        UIQuestText.Clear();

        for (int i = 0; i < questList.Length; i++) {
            Quest quest = questList[i];
            if (quest.hasAccepted) {
                if (quest.currentAmount > quest.totalAmount) {
                    UIQuestText.Add(quest.UITextDisplay + quest.totalAmount + " / " + quest.totalAmount);
                } else {
                    UIQuestText.Add(quest.UITextDisplay + quest.currentAmount + " / " + quest.totalAmount);
                }

            } else {
                //UIQuestText.Add("");
            }
        }

        currentUIQuestText = "";

        foreach (string text in UIQuestText) {
            currentUIQuestText += text + "\n";
            Debug.Log(currentUIQuestText);
        }

        enemiesRemaining.text = currentUIQuestText;

        //if (UIQuestText == "") {
        //    UIQuestText = quest.UITextDisplay + quest.currentAmount + " / " + quest.totalAmount;
        //} else {
        //    UIQuestText += "\n" + quest.UITextDisplay + quest.currentAmount + " / " + quest.totalAmount;
        //}
    }

    public void AcceptQuest(int IDcheck) {

        if(IDcheck < questList.Length) {
            questList[IDcheck].hasAccepted = true;
            updateUI();
        } else {
            throw new Exception("Invalid Quest ID");
        }
    }

    public void AddCounter(int questID, int amount) {
        if(questID < questList.Length) {
            questList[questID].currentAmount += amount;
        } else {
            throw new Exception("Invalid Quest ID");
        }
        
    }

}
