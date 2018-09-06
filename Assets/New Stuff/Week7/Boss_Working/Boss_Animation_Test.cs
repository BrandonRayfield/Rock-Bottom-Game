using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Animation_Test : MonoBehaviour {

    public Animator animator;
    public GameObject guitarObject;

    private Vector3 guitarStartPosition;
    private Quaternion guitarStartRotation;
    private Vector3 guitarStartScale;

    private Vector3 guitarAttackPosition;
    private Quaternion guitarAttackRotation;
    private Vector3 guitarAttackScale;

    public GameObject modelSpine;
    public GameObject weaponObjectSlot;
    private int guitarStance;

	// Use this for initialization
	void Start () {
        guitarStartPosition = guitarObject.transform.localPosition;
        guitarStartRotation = guitarObject.transform.localRotation;
        guitarStartScale = guitarObject.transform.localScale;

        guitarAttackPosition = new Vector3(0.0f, 0.0f, 0.0f); // This is only a rough position, might need to be tuned a bit in order to line it up correctly with hands
        guitarAttackRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f); // This is only a rough rotation, might need to be tuned a bit in order to line it up correctly with hands
        guitarAttackScale = new Vector3(1.0f, 1.0f, 1.0f); // This is only a rough scale, might need to be tuned a bit in order to line it up correctly with hands
    }

    private void changeWeaponPosition(int position) {
        if (position == 0) {
            guitarObject.transform.parent = weaponObjectSlot.transform;
            guitarObject.transform.localPosition = guitarAttackPosition;
            guitarObject.transform.localRotation = guitarAttackRotation;
            guitarObject.transform.localScale = guitarAttackScale;
        } else if (position == 1) {
            guitarObject.transform.parent = modelSpine.transform;
            guitarObject.transform.localPosition = guitarStartPosition;
            guitarObject.transform.localRotation = guitarStartRotation;
            guitarObject.transform.localScale = guitarStartScale;
        }
    }

    //Used for animation event (You will need this exactly the same spelling / case / etc.)
    public void setDefaultPosition() {
        changeWeaponPosition(1);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.F1)) {
            animator.Play("Idle");
            changeWeaponPosition(1);
        }

        if (Input.GetKey(KeyCode.F2)) {
            animator.Play("Charge");
            changeWeaponPosition(1);
        }

        if (Input.GetKey(KeyCode.F3)) {
            animator.Play("Smash");
            changeWeaponPosition(0);
        }

        if (Input.GetKey(KeyCode.F4)) {
            animator.Play("Roar");
            changeWeaponPosition(1);
        }

        if (Input.GetKey(KeyCode.F5)) {
            animator.Play("Jump");
            changeWeaponPosition(1);
        }
    }
}
