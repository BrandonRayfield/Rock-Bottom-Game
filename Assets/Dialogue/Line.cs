using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Line {

    [TextArea]
    public string characterName;

    [TextArea(3,10)]
    public string speech;

    public Texture2D characterPortrait;

    public bool isDecision;
}
