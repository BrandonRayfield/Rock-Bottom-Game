﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObject : MonoBehaviour {

    public float lifeTime = 1.0f;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, lifeTime);
	}
}
