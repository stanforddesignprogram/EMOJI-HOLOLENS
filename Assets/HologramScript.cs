﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("up"))
        {
            print("Up");
        }
        if (Input.GetKey("down"))
        {
            print("Down");
        }
    }
}
