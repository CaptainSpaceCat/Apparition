using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterTriangle {

	private GameObject triangle;
	private Vector3 initialPosition;
	private Vector3 finalPosition;
	private Quaternion initialRotation;

	private Vector3 motionVector;
	private float distanceScale;

	//constructor, creates a new single triangle on the mesh of the room
	public ShatterTriangle(GameObject g) {
		triangle = g;
		initialPosition = triangle.transform.localPosition;
		initialRotation = triangle.transform.rotation;
		newMotionVector();
		setEnabled(false);
	}

	//helper function that enables/disabled the triangle objects
	//used in conjunction with the room objects,
	//so that either the room objects or the triangles are rendered, never both
	public void setEnabled(bool enabled) {
		triangle.GetComponent<MeshRenderer> ().enabled = enabled;
	}

	//sets the new directions for rotational and translational motion for shattering
	public void newMotionVector() {
		distanceScale = Random.Range(5, 9);
		motionVector = Random.onUnitSphere;
		finalPosition = initialPosition + motionVector * distanceScale;
	}

	//sets this object to the correct rotational and translational positions from 0 to 1,
	//both 0 and 1 being its normal position and rotation and everything in between being a 360 spin/direct translation
	public void setInterpolatedTransform(float t, float d) {
		triangle.transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, d);//(float)(-4 * ((t - .5) * (t - .5)) + 1));
		triangle.transform.rotation = initialRotation * Quaternion.AngleAxis(t*360, motionVector);
	}

	public void resetTransform() {
		triangle.transform.localPosition = initialPosition;
		triangle.transform.rotation = initialRotation;
		newMotionVector();
	}

}
