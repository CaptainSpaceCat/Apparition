using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomShatter : MonoBehaviour {

	public AnimationCurve trianglePositionCurve;
	public AnimationCurve rotationCurve;

	private ShatterableObject[] children;
	private Vector3 initialPosition;
	private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
		Transform[] ts = GetComponentsInChildren<Transform> ();
		List<ShatterableObject> childList = new List<ShatterableObject>();
		for (int i = 0; i < ts.Length ; i++) {
			if (ts[i].gameObject.GetComponent<MeshRenderer>()) {
				childList.Add(new ShatterableObject(ts[i].gameObject));
			}
		}

		children = childList.ToArray();

		initialPosition = transform.position;
		initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	bool active = false;
	void Update () {
		
		if (Input.GetKeyUp (KeyCode.Space) && !active) {
			active = true;
			StartCoroutine(shatter(1.5f));
		}

	}

	//coroutine used to shatter the room and rotate it around
	//used during a teleport
	public IEnumerator shatter(float timePeriod) {
		setAllTrianglesEnabled (true);
		//TODO:shatter and rotate the room
		float t = 0f;
		Vector3 motionVector = Random.onUnitSphere;
		while (t < 1) {
			t += Time.deltaTime/timePeriod;
			transform.rotation = initialRotation * Quaternion.AngleAxis(rotationCurve.Evaluate(t)*360, motionVector);
	
			foreach(ShatterableObject sh in children) {
				sh.setInterpolatedTransform(rotationCurve.Evaluate(t), trianglePositionCurve.Evaluate(t));
			}
			yield return null;
		}
		transform.position = initialPosition;
		transform.rotation = initialRotation;
		foreach (ShatterableObject sh in children) {
			sh.resetTransform ();
		}
		setAllTrianglesEnabled (false);
		active = false;
	}

	//sets all triangles in children objects to be enabled
	//used before a teleport to enable needed triangles, used after to disable
	private void setAllTrianglesEnabled(bool enabled) {
		foreach (ShatterableObject s in children) {
			s.setTrianglesEnabled (enabled);
		}
	}

}
