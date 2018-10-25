using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaphone_Script : Weapon {

    public int clipSize;
    public float bulletSpeed;
    public GameObject projectileObject;
    public int ammoCount;

    //Magic
    public GameObject shield;
    private GameObject createdShield;
    private float grenadeSpeed;

    private GameObject grenade;

    // Special Attack 2 Variables
    public GameObject lightningControl;
    public GameObject knockBackObject;

    // Explosive Variables
    public bool isExplosive;
    public GameObject grenadeObject;

    // Sound Variables
    private float currentShootTime;
    private float soundWaitDuration = 6.0f;
    public GameObject shootSound;

    //Animation Variables
    private bool UpdatePositon;

    protected override void Start() {
       base.Start();

        currentShootTime = soundWaitDuration;
        attackAnimationName = "";
    }

    protected override void Update() {
        AdjustWeaponPosition();

        //attack
        if (canAttack) {
            if (Input.GetMouseButtonDown(0) && Time.time > attackTimer) {
                //Debug.Log("Player Attacked");
                animator.SetTrigger(attackAnimationName);
                animator.speed = 2;
                Damage();

                attackTimer = Time.time + attackRate;
            }
            //Lightning Attack
            if (Input.GetKeyDown("x")) {
                if (canUse1 && Time.time > magicTimer1) {
                    SpecialAttack1();
                    animator.speed = 1;
                    playerObject.GetComponent<Player>().updateAbilityCooldown1(magicTimer1);
                    //magicTimer1 = Time.time + magicRate1;
                } else if(grenade != null) {
                    grenade.GetComponent<Projectile_Script>().Detonate();
                } else if (!canUse1 || Time.time < magicTimer1) {
                    Instantiate(errorSound, transform.position, transform.rotation);
                }
            }

            if (Input.GetKeyDown("c")) {
                if (canUse2 && Time.time > magicTimer2) {
                    SpecialAttack2();
                    animator.speed = 1;
                    //magicTimer2 = Time.time + magicRate2;
                    playerObject.GetComponent<Player>().updateAbilityCooldown2(magicTimer2);
                } else if (!canUse2 || (Time.time < magicTimer2)) {
                    Instantiate(errorSound, transform.position, transform.rotation);
                }
            }

        }

        currentShootTime += Time.deltaTime;

        if(UpdatePositon) {
            StartCoroutine(resetPosition());
        } else {
            StopCoroutine(resetPosition());
        }

    }

    protected override void Damage() {

        guitarStance = 1;
        Invoke("ShootBullet", 0.05f);

    }

    private void ShootBullet() {

        if (currentShootTime >= soundWaitDuration) {
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

        GameObject bullet = Instantiate(projectileObject, damageLocation.transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        bullet.GetComponent<Projectile_Script>().SetDamage(weaponDamage);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * bulletSpeed);

        UpdatePositon = true;
    }

    protected override void AdjustWeaponPosition() {
        // Determines what harmonica object is enabled. 0 for Back enabled, 1 for stage, 2 for attack
        if (guitarStance == 1) {
            // Adjusts the harmonica object parent so it moves with the body
            this.gameObject.transform.parent = playerModel.transform;
            // Adjusts the harmonica objects position and rotation
            this.gameObject.transform.localPosition = new Vector3(-0.122f, 1.373f, 1.007f);
            this.gameObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        } else if (guitarStance == 2) {
            // Adjusts the harmonica object parent so it moves with the body
            this.gameObject.transform.parent = playerModel.transform;

            // Adjusts the harmonica object parent so it moves with the body
            this.gameObject.transform.localPosition = new Vector3(-0.122f, 1.373f, 1.007f);
            this.gameObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        } else {
            // Adjusts the harmonica objects position and rotation
            // Adjusts the harmonica object parent so it moves with the body
            this.gameObject.transform.parent = guitarParentSpine.transform;

            this.gameObject.transform.localPosition = new Vector3(0.08808966f, -0.4134668f, -0.2960191f);
            this.gameObject.transform.localEulerAngles = new Vector3(-12.939f, -104.629f, 5.884f);
        }
    }

    protected override void SpecialAttack1() {
        guitarStance = 1;
        Invoke("LaunchGrenade", 0.05f);
    }

    private void LaunchGrenade() {

        Instantiate(attackSound, transform.position, transform.rotation);

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

        grenade = Instantiate(grenadeObject, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        grenade.GetComponent<Projectile_Script>().SetDamage(weaponDamage);
        grenade.GetComponent<Rigidbody>().AddForce(grenade.transform.right * (bulletSpeed + grenadeSpeed));
        magicTimer1 = Time.time + magicRate1;
        UpdatePositon = true;
    }

    protected override void SpecialAttack2() {
        guitarStance = 1;
        Invoke("KnockBack", 0.05f);
    }

    private void KnockBack() {

        Instantiate(attackSound, transform.position, transform.rotation);

        Vector3 mousePos;
        Vector3 attackPos = damageLocation.transform.position;
        float angle;

        mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(Camera.main.transform.position, transform.position);
        attackPos = Camera.main.WorldToScreenPoint(attackPos);
        mousePos.x -= attackPos.x;
        mousePos.y -= attackPos.y;

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(knockBackObject, damageLocation.transform.position, Quaternion.Euler(new Vector3(0, 180, -angle)));

        //List<EnemyGeneric> enemies = lightningControl.GetComponent<ThunderCollider>().InRange;

        //for (int i = 0; i < enemies.Count; i++) {
        //    enemies[i].GetComponent<EnemyGeneric>().CopyrightNeutralBoop(playerObject.transform.position);
        //}

        magicTimer2 = Time.time + magicRate1;
        UpdatePositon = true;
    }



    private IEnumerator resetPosition() {
        yield return new WaitForSeconds(0.5f);
        guitarStance = 0;
        UpdatePositon = false;
        StopAllCoroutines();
    }

}
