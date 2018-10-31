using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomShatter : MonoBehaviour {

	public float shatterTime;

	public AnimationCurve trianglePositionCurve;
	public AnimationCurve rotationCurve;
	public AnimationCurve playerPositionCurve;

	private ShatterableObject[] children;
	private Vector3 initialPosition;
	private Quaternion initialRotation;

	public GameObject playerObject;

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

	bool active = false;
	public void activateShatter() {
		if (!active) {
			active = true;
			StartCoroutine(teleportShatter(shatterTime));
		}
	}

	//coroutine used to shatter the room and rotate it around
	//used during a teleport
	public IEnumerator teleportShatter(float timePeriod) {
		setAllTrianglesEnabled (true);
		//TODO: disable objects that fall so that they don't do so while shattering occurs

		float t = 0f;
		Vector3 motionVector = Random.onUnitSphere;
		Vector3 playerInitialPosition = playerObject.transform.position;
		Vector3 playerFinalPosition = playerObject.GetComponent<PlayerController>().teleportDestination;

		foreach(ShatterableObject sh in children) {
			sh.setPhysicsEnabled(false);
		}

		while (t < 1) {
			t += Time.deltaTime/timePeriod;
			float rot_t = rotationCurve.Evaluate(t);
			float pos_t = trianglePositionCurve.Evaluate(t);
			float player_t = playerPositionCurve.Evaluate(t);
			transform.rotation = initialRotation * Quaternion.AngleAxis(rot_t*360, motionVector);

			playerObject.transform.position = Vector3.Lerp(playerInitialPosition, playerFinalPosition, player_t);

			if (rot_t > .25) {
				//start moving player object
				//we want to start and finish this motion while the room is partway through the shatter
				//this is because early and late sections of the shattering are much slower,
				//so as to improve the flow and fluidity of the animation
				//so slow, that the player will notice if they start moving right at the start
			}

			foreach(ShatterableObject sh in children) {
				sh.setInterpolatedTransform(rot_t, pos_t);
			}


			yield return null;
		}

		//reset the room
		transform.position = initialPosition;
		transform.rotation = initialRotation;
		foreach (ShatterableObject sh in children) {
			sh.resetTransform ();
		}
		setAllTrianglesEnabled (false);
		foreach(ShatterableObject sh in children) {
			sh.setPhysicsEnabled(true);
		}
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
