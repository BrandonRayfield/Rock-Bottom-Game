using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningChain : MonoBehaviour {

    public GameObject lightningEffect;

    private bool isChecking;
    private GameObject target;
    private Vector3 targetPos;
    private float angle;

    private GameObject parent;

    public float enemyRestruckDelay = 5f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider otherObject) {
        if (otherObject.transform.tag == "Enemy" || otherObject.transform.tag == "FlyingEnemy" || otherObject.transform.tag == "Ranged_Enemy" 
                                        || otherObject.transform.tag == "Ranged_Enemy2" || otherObject.transform.tag == "Chaser") {
            CheckChain(otherObject);
        }
    }

    private void CheckChain(Collider targetObject) {
    Struck hit = targetObject.gameObject.GetComponent<Struck>();

    if (hit == null) { // the enemy is yet to be hit
        Debug.Log("Tagged Enemy");

        target = targetObject.gameObject;
        if (target != null) {
            targetPos = targetObject.transform.position;
            SpawnChain(transform.position, targetPos);
        }

        //Create another copy of this lightning field, by doing this, it will start chaining when the condition is right
        Instantiate(gameObject, targetObject.gameObject.transform.position, Quaternion.identity);

        //Mark the enemy as hit
        hit = targetObject.gameObject.AddComponent<Struck>();
        hit.killDelay = enemyRestruckDelay;
            //Kill this gameObject once you have struck the closest enemy
            //Remove the Kill() if you want to strike everyone in the proximity
            //Kill();
        }
    }

    void SpawnChain(Vector3 startPos, Vector3 endPos) {
        GameObject lightningChain = Instantiate(lightningEffect, transform.position, transform.rotation);

        var dir = endPos - startPos;
        lightningChain.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);

        Quaternion originalRot = lightningChain.transform.rotation;
        lightningChain.transform.rotation = originalRot * Quaternion.AngleAxis(90, Vector3.up);

        Vector3 scale = lightningChain.transform.localScale;
        scale.z = dir.magnitude * 0.25f;
        lightningChain.transform.localScale = scale;

    }

    // Also called in an animation event, in case the sphere strikes nothing at all
    void Kill() {
        Destroy(gameObject);
    }

    public void SetParent(GameObject newParent) {
        parent = newParent;
    }

}
