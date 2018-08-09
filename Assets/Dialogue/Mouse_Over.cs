using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse_Over : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public GameObject guitarObject;

    public void OnPointerEnter(PointerEventData eventData) {
        //Debug.Log("Mouse has entered");
        guitarObject.GetComponent<Weapon>().setCanAttack(false);
    }

    public void OnPointerExit(PointerEventData eventData) {
        //Debug.Log("Mouse has exited");
        guitarObject.GetComponent<Weapon>().setCanAttack(true);
    }
}
