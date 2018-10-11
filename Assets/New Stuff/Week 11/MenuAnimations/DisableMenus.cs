using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMenus : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject loadMenu;

    public void DisableCurrentMenu() {
        mainMenu.SetActive(false);
    }

    public void DisableLoadMenu() {
        loadMenu.SetActive(false);
    }
}
