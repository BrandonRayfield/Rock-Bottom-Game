using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {

    public int ID;

    [TextArea(3, 10)]
    public string Description;

    [TextArea(3, 10)]
    public string UITextDisplay;

    public Sprite questIcon;

    public bool isKillQuest;

    public bool isCollectQuest;

    public bool isDestroyQuest;

    public string objectTag;

    [HideInInspector]
    public bool hasAccepted;
    [HideInInspector]
    public bool isComplete;
    [HideInInspector]
    public int totalAmount;
    [HideInInspector]
    public int currentAmount;
    [HideInInspector]
    public int previousAmount;
    [HideInInspector]
    public GameObject targetObject;

}
