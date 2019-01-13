using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCam : MonoBehaviour {

	public Camera Cam;

    public float x;
    public float y;
	// Use this for initialization
	void Start () {
		Cam.clearFlags = CameraClearFlags.Skybox;
		Cam.rect = new Rect(x, y, 0.5f, 0.5f);
		Cam.clearFlags = CameraClearFlags.Depth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
