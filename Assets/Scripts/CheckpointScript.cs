using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckpointScript : MonoBehaviour {

    public Text checkpointText;
    private bool firstTimecheckpointReached = false;
    private bool textDisappeared = false;
    private float time;
    private float maxTime = 3.0f;

    public string message = "Checkpoint Reached!";


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (textDisappeared)
        {
            time = 0;
        }

        if (firstTimecheckpointReached && !textDisappeared) {
            time += Time.deltaTime;

            if (time > maxTime)
            {
                checkpointText.enabled = false;
                textDisappeared = true;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !firstTimecheckpointReached)
        {
            time = 0;
            checkpointText.text = message;
            checkpointText.enabled = true;
            firstTimecheckpointReached = true;
            textDisappeared = false;
        }
    }
}
