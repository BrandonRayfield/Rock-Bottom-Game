using System.Collections;
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

    //Animation Variables
    private bool UpdatePositon;

    protected override void Start() {
        base.Start();

        currentShootTime = soundWaitDuration;
        attackAnimationName = "";
    }

    protected override void Update() {
        base.Update();

        currentShootTime += Time.deltaTime;

        if (UpdatePositon) {
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

        GameObject bullet = Instantiate(projectileObject, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

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
            this.gameObject.transform.localPosition = new Vector3(0.02f, 2.057f, 0.723f);
            this.gameObject.transform.localEulerAngles = new Vector3(0.0f, -90.00001f, 0.0f);

        } else if (guitarStance == 2) {
            // Adjusts the harmonica object parent so it moves with the body
            this.gameObject.transform.parent = playerModel.transform;
            
            // Adjusts the harmonica objects position and rotation
            this.gameObject.transform.localPosition = new Vector3(0.02f, 2.057f, 0.723f);
            this.gameObject.transform.localEulerAngles = new Vector3(0f, -90.00001f, 0f);

        } else {
            // Adjusts the harmonica object parent so it moves with the body
            this.gameObject.transform.parent = guitarParentSpine.transform;

            // Adjusts the harmonica objects position and rotation
            this.gameObject.transform.localPosition = new Vector3(0.332f, -0.362f, 0.177f);
            this.gameObject.transform.localEulerAngles = new Vector3(-126.638f, -212.314f, 82.323f);
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

        GameObject bullet = Instantiate(grenadeObject, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        bullet.GetComponent<Projectile_Script>().SetDamage(weaponDamage);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * (bulletSpeed + grenadeSpeed));
        magicTimer1 = Time.time + magicRate1;
        UpdatePositon = true;
    }

    private IEnumerator resetPosition() {
        yield return new WaitForSeconds(0.5f);
        guitarStance = 0;
        UpdatePositon = false;
        StopAllCoroutines();
    }

}
