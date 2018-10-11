using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThunderCollider : MonoBehaviour {

    public List<EnemyGeneric> InRange = new List<EnemyGeneric>();

    public Player player;

    //Temp Lightning attack variables
    public float magicTimer;
    public float magicRate = 1.5f;
    public GameObject lightningPrefab;
    public float damage = 100f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Controls();
        checkDead();
    }

    public void enterRange(EnemyGeneric enemy) {
        //Get the count which will be the index of the newly added enemy
        int length = (InRange.Count);
        //Add the enemy (at index length (because indexes start at 0))
        InRange.Add(enemy);
        enemy.LightningChange(length);
    }


    public void removeEnemy(int location/*the location contained within the enemy itself*/) {
            //Inform the enemy that it will not part of the list
            InRange[location].LightningChange(-1);
            //Remove the index the enemy has been told it has
            InRange.RemoveAt(location);

        //For each enemy in the list that it has been moved
        for (int i = location; i < InRange.Count; i++) {
                //The enemy has already been moved in the list just inform it that it has
                InRange[i].LightningChange(i);
        }
    }
    public void OnTriggerEnter(Collider other) {
        //If it's an enemy
        if (other.tag == "Enemy" || other.tag == "FlyingEnemy" || other.tag == "Chaser") {
            //Add the enemy to the list
            enterRange(other.GetComponent<EnemyGeneric>());
            checkDuplicates();
        }
    }


    public void OnTriggerExit(Collider other) {
        //If it's an enemy
        if (other.tag == "Enemy" || other.tag == "FlyingEnemy" || other.tag == "Chaser") {
            //Add the enemy to the list
            if (other.GetComponent<EnemyGeneric>().lightningListLocation != -1) {
                removeEnemy(other.GetComponent<EnemyGeneric>().lightningListLocation);
                //other.GetComponent<Enemy>().Lightnin gChange(-1);
            }
        }
    }

    public Vector3 findClosest() {
        float optimalDistance = 1000000;
        EnemyGeneric target = null;
        for (int i = 0; i < InRange.Count; i++) {
            float distance = Vector3.Distance(player.transform.position, InRange[i].transform.position);
            if(distance < optimalDistance) {
                optimalDistance = distance;
                target = InRange[i];
            }
        }
        if (target != null) {
            return target.transform.position;
        } else {
            return new Vector3 (0f,0f,0f);
        }
    }

 public void checkDead() {

        for (int i = 0; i < InRange.Count; i++) {

            if (InRange[i].health <= 0) InRange.Remove(InRange[i]);
        }
    }

    public void checkDuplicates() {
        for (int i = 0; i < InRange.Count; i++) {

            for (int j = 0; j < InRange.Count; j++) {

                if (InRange[i].gameObject == InRange[j].gameObject && i != j) InRange.Remove(InRange[j]);
            }
        }
    }
}
