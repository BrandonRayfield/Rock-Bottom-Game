﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harmonica_Script : Weapon {

    public int clipSize;
    public float bulletSpeed;
    public GameObject projectileObject;
    public int ammoCount;

    //Magic
    public GameObject shield;
    private GameObject createdShield;
    private float grenadeSpeed;

    // Explosive Variables
    public bool isExplosive;
    public GameObject grenadeObject;

    // Sound Variables
    private float currentShootTime;
    private float soundWaitDuration = 6.0f;
    public GameObject shootSound;

    protected override void Start() {
        base.Start();

        currentShootTime = soundWaitDuration;
    }

    protected override void Update() {
        base.Update();

        currentShootTime += Time.deltaTime;
        attackAnimationName = "";
    }


    protected override void Damage() {

        if(currentShootTime >= soundWaitDuration) {
            Instantiate(attackSound, transform.position, transform.rotation);
            currentShootTime = 0;
        } else {
            Instantiate(shootSound, transform.position, transform.rotation);
            currentShootTime = 0;
        }

        Vector3 mousePos;
        Vector3 attackPos = damageLocation.transform.position;
        float angle;

        mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(Camera.main.transform.position, transform.position);
        attackPos = Camera.main.WorldToScreenPoint(attackPos);
        mousePos.x -= attackPos.x;
        mousePos.y -= attackPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(projectileObject, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        bullet.GetComponent<Projectile_Script>().SetDamage(weaponDamage);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * bulletSpeed);

    }

    protected override void AdjustWeaponPosition() {
        // Determines what harmonica object is enabled. 0 for Back enabled, 1 for stage, 2 for attack
        if (guitarStance == 1) {
            // Adjusts the harmonica objects position and rotation
            //this.gameObject.transform.localPosition = new Vector3(0.02f, 2.057f, 0.779f);
            //this.gameObject.transform.localEulerAngles = new Vector3(0.0f, -90.00001f, 0.0f);

            // Adjusts the harmonica object parent so it moves with the body
            //this.gameObject.transform.parent = guitarParentSpine.transform;

        } else if (guitarStance == 2) {
            // Adjusts the harmonica object parent so it moves with the body
            //this.gameObject.transform.parent = guitarParentHand.transform;
            
            // Adjusts the harmonica objects position and rotation
            //this.gameObject.transform.localPosition = new Vector3(-0.4303697f, 0.6051218f, 0.6971588f);
            //this.gameObject.transform.localEulerAngles = new Vector3(2.145f, 51.479f, 0.791f);

        } else {
            // Adjusts the harmonica objects position and rotation
            //this.gameObject.transform.localPosition = new Vector3(-0.2714013f, -0.1477957f, -0.0788268f);
            //this.gameObject.transform.localEulerAngles = new Vector3(65.565f, -37.631f, 85.8f);


            // Adjusts the harmonica object parent so it moves with the body
            //this.gameObject.transform.parent = guitarParentSpine.transform;
        }
    }

    protected override void SpecialAttack1() {

        if (!isExplosive) {
            createdShield = Instantiate(shield, transform.parent.transform.position, transform.parent.transform.rotation);

            createdShield.transform.parent = gameObject.transform.parent;
            createdShield.transform.position += new Vector3(0f, 1f, -0.4f);
            playerObject.GetComponent<Player>().invulnerable = true;
            magicTimer1 = Time.time + magicRate1;
        } else {
            Vector3 mousePos;
            Vector3 attackPos = damageLocation.transform.position;
            float angle;

            mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(Camera.main.transform.position, transform.position);
            attackPos = Camera.main.WorldToScreenPoint(attackPos);
            mousePos.x -= attackPos.x;
            mousePos.y -= attackPos.y;

            angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            if (angle >= 20) {
                grenadeSpeed = 150;
            } else {
                grenadeSpeed = 50;
            }

            Debug.Log(angle);

            GameObject bullet = Instantiate(grenadeObject, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

            bullet.GetComponent<Projectile_Script>().SetDamage(weaponDamage);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * (bulletSpeed + grenadeSpeed));
            magicTimer1 = Time.time + magicRate1;
        }

        
    }
}
