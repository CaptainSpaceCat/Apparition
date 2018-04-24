using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject look;
	private Transform cam;
	// Use this for initialization
	void Start () {
		cam = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		cam.LookAt (look.transform.position);
	}
}
