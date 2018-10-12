using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarryOver : MonoBehaviour {

    public int coins = 0;
    public int lives = 3;
    public bool carried = false;
    public Player player;
    public int scene = 0;


	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame crazy modifications woah
	void Update () {
	    if (SceneManager.GetActiveScene().buildIndex != 0 && !carried) {
            carried = true;
        } else if (SceneManager.GetActiveScene().buildIndex == 0 && carried) {
            Destroy(this.gameObject);
        }

        if (SceneManager.GetActiveScene().buildIndex != scene && SceneManager.GetActiveScene().buildIndex != 0) {
            scene = SceneManager.GetActiveScene().buildIndex;
            player = (Player)FindObjectOfType(typeof(Player));
            player.GetComponent<Player>().currencyCount = coins;
            player.GetComponent<Player>().livesLeft = lives;
            player.GetComponent<Player>().currencyText.text = "Beat Coins: " + currencyCount.ToString();
            player.GetComponent<Player>().livesText.text = "Lives: " + livesLeft;
        } else if (SceneManager.GetActiveScene().buildIndex != 0) {
            coins = player.GetComponent<Player>().currencyCount;
            lives = player.GetComponent<Player>().livesLeft;
        }

    }
}
