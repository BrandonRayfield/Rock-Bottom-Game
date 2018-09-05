using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPrompt : MonoBehaviour {

	public string Text;
	public Text interactText;
	public GameObject breakable_Rock;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			interactText.enabled = true;
			interactText.text = Text;
		}

		if(breakable_Rock == null) {
			interactText.enabled = false;
			interactText.text = "";
		}
	}

	private void OnTriggerStay(Collider other) {
		if(other.gameObject.tag == "Player") {
				interactText.enabled = true;
				interactText.text = Text;
			} 

		if(breakable_Rock == null) {
			interactText.enabled = false;
			interactText.text = "";
		}
		}


	private void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			interactText.enabled = false;
			interactText.text = "";
		}

		if(breakable_Rock == null) {
			interactText.enabled = false;
			interactText.text = "";
		}
	}
		
}

