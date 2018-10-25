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

    //Expression Variable
    private int expressionValue;

    //Dialogue variables
    public bool isTalking = false;

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
                }

                quest.previousAmount = quest.currentAmount;
            } else if (quest.isKillQuest) {
                if (quest.totalAmount == 0) {
                    quest.totalAmount = GetAmountTotal(quest.objectTag);
                }

                quest.previousAmount = quest.currentAmount;
            } else if (quest.isDestroyQuest) {
                //quest.targetObject = GameObject.FindGameObjectWithTag(quest.objectTag);
            }
        }

        //Collider setup (bosses move through player)
        Physics.IgnoreLayerCollision(8, 10);
        Physics.IgnoreLayerCollision(8, 11);
        Physics.IgnoreLayerCollision(11, 11);
        Physics.IgnoreLayerCollision(10, 11);
        Physics.IgnoreLayerCollision(10, 12);
        Physics.IgnoreLayerCollision(10, 13);
        Physics.IgnoreLayerCollision(12, 13);

        Physics.IgnoreLayerCollision(15, 8);
        Physics.IgnoreLayerCollision(15, 13);
    }
	
	// Update is called once per frame
	void Update () {

    }

    // Used to check how many objects / kills a player needs for a quest
    public int GetAmountTotal(string objectTag) {
        questObjects = GameObject.FindGameObjectsWithTag(objectTag);
        return questObjects.Length;
    }

    // Used to check if the player has completed a quest yet
    public bool GetIsComplete(int IDcheck) {

        if(IDcheck < questList.Length) {
            return questList[IDcheck].isComplete;
        } else {
            throw new Exception("Invalid Quest ID");
        }
    }

    // Used to check if the player has accepted a quest yet
    public bool GetHasAccepted(int IDcheck) {

        if (IDcheck < questList.Length) {
            return questList[IDcheck].hasAccepted;
        }
        else {
            throw new Exception("Invalid Quest ID");
        }
    }

    // Used to update the UI when a quest is updated
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
        }

        enemiesRemaining.text = currentUIQuestText;
    }

    //Used to accept a quest
    public void AcceptQuest(int IDcheck) {

        if(IDcheck < questList.Length) {
            questList[IDcheck].hasAccepted = true;
            updateUI();
        } else {
            throw new Exception("Invalid Quest ID");
        }
    }

    // Used to update a quest when it is completed by the player
    public void CompletedQuest(int IDcheck) {
        if (IDcheck < questList.Length) {
            questList[IDcheck].isComplete = true;
            updateUI();

        } else {
            throw new Exception("Invalid Quest ID");
        }
    }

    // Adds an specific ammount to the current amount (Used for most quest types)
    public void AddCounter(int questID, int amount) {
        if(questID < questList.Length) {
            questList[questID].currentAmount += amount;
            updateUI();

            //Check if quest is complete
            if (questList[questID].currentAmount >= questList[questID].totalAmount) {
                questList[questID].isComplete = true;
            }

        } else {
            throw new Exception("Invalid Quest ID");
        }
        
    }

    // Sets the current amount to a specific amount (Used for target quests)
    public void SetCounter(int questID, int amount) {
        if (questID < questList.Length) {
            questList[questID].currentAmount = amount;
            updateUI();

            //Check if quest is complete
            if (questList[questID].currentAmount >= questList[questID].totalAmount) {
                questList[questID].isComplete = true;
            }

        } else {
            throw new Exception("Invalid Quest ID");
        }
    }

    public int getExpressionType(int questID) {
        if(questID < questList.Length) {
            if (questList[questID].isComplete) {
                return 2;
            } else {
                return 0;
            }
        } else {
            return 0;
        }
    }

    //Used to kill all spiders in a scene (used in transition and boss fight scenes)
    public void killTheSpiders() {
        GameObject[] spiders = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < spiders.Length; i++) {
            if (spiders[i].GetComponent<Chaser>() != null) spiders[i].GetComponent<Chaser>().takeDamage(100);
        }
    }
}
