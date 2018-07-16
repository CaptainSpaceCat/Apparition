using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunArc : MonoBehaviour
{

	public LineRenderer teleArc;
	public LineRenderer teleSpot;
	//public LineRenderer teleRing;
	Vector3[] arcPos;

	bool inAir ()
	{
		RaycastHit hit;
		Physics.Raycast (GetComponent<Rigidbody>().transform.position, Vector3.down, out hit);
		if (hit.distance > .5) {
			return true;
		}
		return false;
	}

	void drawLine (Vector3[] hit)
	{

		/*if (hit == Vector3.zero) {
            teleArc.enabled = false;
        } else {*/
		/*Vector3[] arc = {
		rigidbody.position + transform.right * (float).2 + transform.up * (float)-.2,
		hit,
	};*/
		teleArc.SetPositions (hit);
		teleArc.enabled = true;


		//}
	}

	void drawRing (RaycastHit hit, float scale, float offset)
	{
		/*Vector3[] ring = new Vector3[] {
		new Vector3(1, 0, 0) * scale + hit.point + hit.normal * s,
		new Vector3(1, 0, 1).normalized * scale + hit.point + hit.normal * s,
		new Vector3(0, 0, 1) * scale + hit.point + hit.normal * s,
		new Vector3(-1, 0, 1).normalized * scale + hit.point + hit.normal * s,
		new Vector3(-1, 0, 0) * scale + hit.point + hit.normal * s,
		new Vector3(-1, 0, -1).normalized * scale + hit.point + hit.normal * s,
		new Vector3(0, 0, -1) * scale + hit.point + hit.normal * s,
		new Vector3(1, 0, -1).normalized * scale + hit.point + hit.normal * s,
	};*/
		Vector3[] ring = new Vector3[8];
		Vector3 a, b;
		for (int i = 0; i < 8; i++) {

		}

		//teleRing.SetPositions (ring);
		//teleRing.positionCount = ring.Length;
	}
	//12
	RaycastHit getGunArc (Vector3 pos, Vector3 velocity, float vScale, Vector3 accel)
	{
		velocity *= vScale;



		RaycastHit hit;


		//Vector3 dir = rigidbody.transform.forward;
		//dir.Normalize();  //unneccesary, as rigidbody.transform.forward is always normalized
		//Vector3 initialVelocity = dir * velocity;
		float time = 0.0f;
		float timeStep = .1f;

		const int MAX_SEGMENTS = 30;
		arcPos = new Vector3[MAX_SEGMENTS];
		//bool hitObject = false;
		int count = 0;
		while (time < (MAX_SEGMENTS - 1) * timeStep) {
			Vector3 newPos;

			newPos.x = (float)(pos.x + (velocity.x * timeStep) + (0.5 * accel.x * timeStep * timeStep));
			newPos.y = (float)(pos.y + (velocity.y * timeStep) + (0.5 * accel.y * timeStep * timeStep));
			newPos.z = (float)(pos.z + (velocity.z * timeStep) + (0.5 * accel.z * timeStep * timeStep));

			Vector3 dir = newPos - pos;
			Physics.Raycast (pos, dir, out hit, dir.magnitude);

			if (hit.point != Vector3.zero) {
				arcPos [count] = pos;
				count++;
				//arcPos [count + 1] = hit.point;

				for (int i = count; i < MAX_SEGMENTS; i++) {
					arcPos [i] = hit.point;
				}
				teleArc.positionCount = MAX_SEGMENTS;
				drawLine (arcPos);

				Vector3[] spot = new Vector3[2];
				spot [0] = hit.point;
				spot [1] = spot [0] + hit.normal * 3.5f;
				teleSpot.positionCount = 2;
				teleSpot.SetPositions (spot);


				drawRing (hit, .5f, .01f);

				return hit;
			} else {
				arcPos [count] = pos;
				pos = newPos;


			}
			velocity.x = velocity.x + accel.x * timeStep;
			velocity.y = velocity.y + accel.y * timeStep;
			velocity.z = velocity.z + accel.z * timeStep;

			time += timeStep;
			count++;
		}


		teleArc.enabled = false;
		return new RaycastHit ();
	}


	public float gunVelocity;

	public Vector3 gravity = new Vector3 (0f, -9.81f, 0f);

	// Use this for initialization
	void FixedUpdate ()
	{
		RaycastHit hit = getGunArc (GetComponent<Rigidbody>().transform.position, GetComponent<Rigidbody>().transform.forward, 12f, gravity);
		if (Input.GetMouseButtonUp (0)) {
			GetComponent<Rigidbody>().position = hit.point;
			//gravity = hit.normal * -9.81f;
		}
	}
}
