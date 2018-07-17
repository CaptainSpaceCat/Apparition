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

	public ShatterTriangle(GameObject g) {
		triangle = g;
		initialPosition = triangle.transform.localPosition;
		initialRotation = triangle.transform.rotation;
		newMotionVector();
		setEnabled(false);
	}

	public void setEnabled(bool enabled) {
		triangle.GetComponent<MeshRenderer> ().enabled = enabled;
	}

	public void newMotionVector() {
		distanceScale = Random.Range(5, 9);
		motionVector = Random.onUnitSphere;
		finalPosition = initialPosition + motionVector * distanceScale;
	}

	public void setInterpolatedTransform(float t, float d) {
		/*if (t < .5) {
			triangle.transform.position = Vector3.Lerp(initialPosition, finalPosition, t);
		} else {
			triangle.transform.position = Vector3.Lerp(finalPosition, initialPosition, t);
		}*/
		triangle.transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, d);//(float)(-4 * ((t - .5) * (t - .5)) + 1));
		triangle.transform.rotation = initialRotation * Quaternion.AngleAxis(t*360, motionVector);
	}

	public void resetTransform() {
		triangle.transform.localPosition = initialPosition;
		triangle.transform.rotation = initialRotation;
		newMotionVector();
	}

}
