using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar_Script : Weapon {

    // Special Attack 1 Variables
    public GameObject lightningControl;
    public GameObject lightningHitbox;
    public GameObject lightningParticles;
    public float lightningDamage = 150;
    private float currentChannelTime;
    public float channelTime = 1.0f; // How long it takes for attack to charge before actually activating
    public Vector3 target;
    private Vector3 nullTarget = new Vector3(0, 0, 0);

    // Special Attack 2 Variables
    public GameObject forceObject;
    public float growTime;
    private float currentGrowTime;

    protected override void Update() {
        base.Update();

        target = lightningControl.GetComponent<ThunderCollider>().findClosest();
        //Debug.Log(target);
        if(target != new Vector3(0, 0, 0)) {
            canUse1 = true;
        } else {
            canUse1 = false;
        }
    }

    protected override void SpecialAttack1() {
        target = lightningControl.GetComponent<ThunderCollider>().findClosest();

        if (target != new Vector3(0,0,0)) {

            //Debug.Log("Target Found");

            currentDirection = playerObject.GetComponent<Player>().getPlayerDirection();
            Debug.Log(currentDirection);

            playerModel.transform.localEulerAngles = new Vector3(0, currentDirection * 90, 0);
            animator.Play("Guitar Playing");
            guitarStance = 1;
            Instantiate(magicSound1, transform.position, transform.rotation);
            // Channel Ability
            setCanAttack(false);
            playerObject.GetComponent<Player>().setCanMove(false);
            Invoke("channelAbility", channelTime);
            magicTimer1 = Time.time + magicRate1;
        } else {
            //Debug.Log("No valid target.");
        }
    }

    protected override void SpecialAttack2() {
        currentDirection = playerObject.GetComponent<Player>().getPlayerDirection();
        playerModel.transform.localEulerAngles = new Vector3(0, currentDirection * 90, 0);
        animator.Play("Guitar Playing");
        guitarStance = 1;
        Instantiate(forceObject, transform.position, new Quaternion(0,0,0,0));
        Instantiate(magicSound1, transform.position, transform.rotation);
        setCanAttack(false);
        playerObject.GetComponent<Player>().setCanMove(false);
        Invoke("channelAbility2", channelTime);
        magicTimer2 = Time.time + magicRate2;
    }

    private void channelAbility() {
        //target = lightningControl.GetComponent<ThunderCollider>().findClosest();
        //target.y += 8.5f;
        lightningAttack(target);
        //currentChannelTime = 0;
        playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
        guitarStance = 0;
        setCanAttack(true);
        playerObject.GetComponent<Player>().setCanMove(true);
        //currentChannelTime = Time.time + channelTime;
    }

    private void channelAbility2() {
        playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
        setCanAttack(true);
        playerObject.GetComponent<Player>().setCanMove(true);
        guitarStance = 0;
    }

    public void lightningAttack(Vector3 target) {
        if (target != null) {
            GameObject prefab = Instantiate(lightningHitbox, target, transform.rotation);
            prefab.GetComponent<DamageHitBox>().damage = lightningDamage;
            prefab.GetComponent<DamageHitBox>().player = true;
            Instantiate(lightningParticles, target, transform.rotation);
            //Instantiate(attackSound, transform.position, transform.rotation);
        }
    }

}
