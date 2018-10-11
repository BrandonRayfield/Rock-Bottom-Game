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
        if (otherObject.transform.tag == "Enemy") {
            Enemy e = otherObject.gameObject.GetComponent<Enemy>();

            if (e == null) {  // It is not enemy
                return;
            }

            Struck hit = otherObject.gameObject.GetComponent<Struck>();

            if (hit == null) { // the enemy is yet to be hit
                target = otherObject.gameObject;
                if (target != null) {
                    targetPos = otherObject.transform.position;
                    SpawnChain(transform.position, targetPos);
                }
                //Create another copy of this lightning field, by doing this, it will start chaining when the condition is right
                Instantiate(gameObject, otherObject.gameObject.transform.position, Quaternion.identity);
                //Mark the enemy as hit
                hit = otherObject.gameObject.AddComponent<Struck>();
                hit.killDelay = enemyRestruckDelay;
                //Kill this gameObject once you have struck the closest enemy
                //Remove the Kill() if you want to strike everyone in the proximity
                Kill();
            }
        }
        //----------------------------------------------------------------------------------
        else if (otherObject.transform.tag == "FlyingEnemy") {
            Flying_Enemy e = otherObject.gameObject.GetComponent<Flying_Enemy>();

            if (e == null) {  // It is not enemy
                Debug.Log("No Enemy");
                return;
            }

            Struck hit = otherObject.gameObject.GetComponent<Struck>();

            if (hit == null) { // the enemy is yet to be hit
                Debug.Log("Tagged Enemy");

                target = otherObject.gameObject;
                if (target != null) {
                    targetPos = otherObject.transform.position;
                    SpawnChain(transform.position, targetPos);
                }

                //Create another copy of this lightning field, by doing this, it will start chaining when the condition is right
                Instantiate(gameObject, otherObject.gameObject.transform.position, Quaternion.identity);

                //Mark the enemy as hit
                hit = otherObject.gameObject.AddComponent<Struck>();
                hit.killDelay = enemyRestruckDelay;
                //Kill this gameObject once you have struck the closest enemy
                //Remove the Kill() if you want to strike everyone in the proximity
                Kill();
            }
        }
        //----------------------------------------------------------------------------------
        else if (otherObject.transform.tag == "Ranged_Enemy") {

        }
        //----------------------------------------------------------------------------------
        else if (otherObject.transform.tag == "Ranged_Enemy2") {

        }
        //----------------------------------------------------------------------------------
        else if (otherObject.transform.tag == "Chaser") {
        }
    }

    void SpawnChain(Vector3 startPos, Vector3 endPos) {
        Debug.Log("Made a new lightning");
        GameObject lightningChain = Instantiate(lightningEffect, transform.position, transform.rotation);
        Vector3 centerPos = new Vector3(startPos.x + endPos.x, startPos.y + endPos.y) / 2;

        float scaleX = Mathf.Abs(startPos.x - endPos.x);
        float scaleY = Mathf.Abs(startPos.y - endPos.y);

        Vector3 newTarget;
        Vector3 currentPos = transform.position;

        newTarget = Input.mousePosition;
        newTarget.z = Vector3.Distance(Camera.main.transform.position, transform.position);
        currentPos = Camera.main.WorldToScreenPoint(currentPos);
        newTarget.x -= currentPos.x;
        newTarget.y -= currentPos.y;
        angle = Mathf.Atan2(newTarget.y, newTarget.x) * Mathf.Rad2Deg;

        lightningChain.transform.rotation = Quaternion.Euler(new Vector3(-angle, 90, 0));

        centerPos.x -= 0.5f;
        centerPos.y += 0.5f;
        lightningChain.transform.position = centerPos;
        lightningChain.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }

    // Also called in an animation event, in case the sphere strikes nothing at all
    void Kill() {
        Destroy(gameObject);
    }

    public void SetParent(GameObject newParent) {
        parent = newParent;
    }

}
