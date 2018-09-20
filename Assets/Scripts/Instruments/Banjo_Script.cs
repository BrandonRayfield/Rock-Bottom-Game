using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banjo_Script : Weapon {

    //Magic Variables
    public GameObject lightningControl;
    public GameObject cowObject;
    public float lightningDamage = 100;
    private float currentChannelTime;
    public float channelTime = 1.0f; // How long it takes for attack to charge before actually activating
    public Vector3 target;
    private Vector3 nullTarget = new Vector3(0, 0, 0);

    protected override void Update() {
        base.Update();

        target = lightningControl.GetComponent<ThunderCollider>().findClosest();
        //Debug.Log(target);
        if (target != new Vector3(0, 0, 0)) {
            canUse1 = true;
        } else {
            canUse1 = false;
        }
    }

    protected override void SpecialAttack1() {
        target = lightningControl.GetComponent<ThunderCollider>().findClosest();

        if (target != nullTarget) {

            Debug.Log("Target Found");

            currentDirection = playerObject.GetComponent<Player>().getPlayerDirection();
            Debug.Log(currentDirection);

            playerModel.transform.localEulerAngles = new Vector3(0, currentDirection * 90, 0);
            animator.Play("Guitar Playing");
            guitarStance = 1;
            Instantiate(magicSound1, transform.position, transform.rotation);
            // Channel Ability
            Invoke("channelAbility", channelTime);
            magicTimer1 = Time.time + magicRate1;
        } else {
            Debug.Log("No valid target.");
        }
    }

    private void channelAbility() {
        target = lightningControl.GetComponent<ThunderCollider>().findClosest();
        //target.y += 8.5f;
        lightningAttack(target);
        //currentChannelTime = 0;
        playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
        guitarStance = 0;
        //currentChannelTime = Time.time + channelTime;
    }

    public void lightningAttack(Vector3 target) {
        if (target != null) {
            GameObject prefab = Instantiate(cowObject, new Vector3(target.x, target.y + 4, target.z), transform.rotation);
            prefab.GetComponent<DamageHitBox>().damage = lightningDamage;
            prefab.GetComponent<DamageHitBox>().player = true;
            //Instantiate(attackSound, transform.position, transform.rotation);
        }
    }

}
