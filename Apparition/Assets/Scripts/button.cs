using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

	public bool isPressed = false; //public for testing purposes
	public float regHeight = .15f;
	public float pressedHeight = .05f;

	void Update () {
		Vector3 scale = gameObject.transform.localScale;
		if (isPressed) {
			scale.y = pressedHeight;
			gameObject.transform.localScale = scale;
		} else {
			scale.y = regHeight;
			gameObject.transform.localScale = scale;
		}
	}

	public void changeState(bool pressed) {
		isPressed = pressed;
	}
}
