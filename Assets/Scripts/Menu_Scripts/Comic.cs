using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Comic {

    public string pageName;

    public Sprite comicImage;

    [TextArea(3, 10)]
    public string dialogue;

    public GameObject sound;

    public float soundDelay;

}