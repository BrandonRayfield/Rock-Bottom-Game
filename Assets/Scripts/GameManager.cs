using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //Singleton Setup
    public static GameManager instance = null;

    public GameObject[] enemies;
	public Text enemiesRemaining;

	public bool goldKey = false;

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
		
	}
	
	// Update is called once per frame
	void Update () {

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

		enemiesRemaining.text = "Enemies Remaining: " + enemies.Length;
	}
}
