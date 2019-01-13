using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour {

	public Camera mapC;
	public Camera playerC;

	// Use this for initialization
	void Start () {
		playerC = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.M)){
			playerC.enabled = false;
		}
		playerC.enabled = true;
	}
}
