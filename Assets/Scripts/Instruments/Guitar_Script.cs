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

    public GameObject lightningSpell;

    private float angle;
    private GameObject bullet;

    //Rotation vars
    private float rotationSpeed = 1.5f;
    private float adjRotSpeed;

    protected override void Update() {
        base.Update();

        Vector3 mousePos;
        Vector3 attackPos = damageLocation.transform.position;

        mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(Camera.main.transform.position, transform.position);
        attackPos = Camera.main.WorldToScreenPoint(attackPos);
        mousePos.x -= attackPos.x;
        mousePos.y -= attackPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (bullet != null) {
            adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
            bullet.gameObject.transform.rotation = Quaternion.Lerp(bullet.transform.rotation, Quaternion.Euler(new Vector3(-angle, 90, 0)), adjRotSpeed);
            //bullet.gameObject.transform.rotation = new Quaternion(Mathf.Lerp(bullet.transform.rotation.x, -angle, adjRotSpeed), bullet.transform.rotation.y, bullet.transform.rotation.z, 1);
        }
    }

    protected override void SpecialAttack1() {
        animator.SetTrigger("Playing");
        Instantiate(magicSound1, transform.position, transform.rotation);
        bullet = Instantiate(lightningSpell, damageLocation.transform.position, Quaternion.Euler(new Vector3(-angle, 90, 0)), damageLocation.gameObject.transform);
        magicTimer1 = Time.time + magicRate1;
    }

    protected override void SpecialAttack2() {
        currentDirection = playerObject.GetComponent<Player>().getPlayerDirection();
        //playerModel.transform.localEulerAngles = new Vector3(0, currentDirection * 90, 0);
        animator.SetTrigger("Playing");
        guitarStance = 1;
        Instantiate(forceObject, transform.position, new Quaternion(0,0,0,0));
        Instantiate(magicSound2, transform.position, transform.rotation);
        //setCanAttack(false);
        //playerObject.GetComponent<Player>().setCanMove(false);
        //Invoke("channelAbility2", channelTime);
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
