using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunArc : MonoBehaviour {

	public LineRenderer teleArc;
	public LineRenderer teleSpot;
	public LineRenderer teleRing;

	public float scalar;

	Vector3[] arcPos;

	void Start() {
		teleArc.enabled = false;
		teleSpot.enabled = false;
		teleRing.enabled = false;
		//fire();
	}
	void Update() {
		fire();
	}

	const int maxSegments = 200;
	//this function fires the gun in an arc, drawing the arc in the air,
	//and identifying the point at which it lands with a beacon
	public void fire() {
		List<Vector3> arcSpots = new List<Vector3>();
		Vector3 pos = transform.position;
		Vector3 dir = transform.forward * scalar;

		arcSpots.Add(transform.position);
		int segments = 1;
		while (!getNextArcSegment(ref pos, ref dir, 5f, Physics.gravity, .1f)) {
			arcSpots.Add(new Vector3(pos.x, pos.y, pos.z));
			segments++;
			if (segments > maxSegments) {
				teleArc.enabled = false;
				teleSpot.enabled = false;
				teleRing.enabled = false;
				return;
			}
		}
		arcSpots.Add(new Vector3(pos.x, pos.y, pos.z));

		drawArc(arcSpots.ToArray());
	}

	//draws an arc based on the passed array of vectors
	private void drawArc(Vector3[] arc) {
		teleArc.positionCount = arc.Length;
		teleArc.SetPositions(arc);
		teleArc.enabled = true;
	}

	//draws the beacon line based on the passed array of vectors
	private void drawLine (Vector3 point, Vector3 normal) {
		Vector3[] line = new Vector3[2];
		line[0] = point;
		line[1] = point + normal * 3;

		teleSpot.positionCount = 2;
		teleSpot.SetPositions(line);
		teleSpot.enabled = true;
	}

	//draws the beacon ring based on the passed array of vectors
	private void drawRing (Vector3 point, Vector3 normal, int numPoints) {
		int firstNonzero;
		if (normal.x != 0) {
			firstNonzero = 0;
		} else if (normal.y != 0) {
			firstNonzero = 1;
		} else {
			firstNonzero = 2;
		}

		Vector3[] orth = new Vector3[2];
		if (firstNonzero == 0) {
			orth[0] = new Vector3(-1 * normal.y, normal.x, 0f).normalized;
			orth[1] = new Vector3(-1 * normal.z, 0f, normal.x).normalized;
		} else if (firstNonzero == 1) {
			orth[0] = new Vector3(normal.y, -1 * normal.x, 0f).normalized;
			orth[1] = new Vector3(0f, -1 * normal.z, normal.y).normalized;
		} else if (firstNonzero == 2) {
			orth[0] = new Vector3(normal.z, 0f, -1 * normal.x).normalized;
			orth[1] = new Vector3(0f, normal.z, -1 * normal.y).normalized;
		}

		List<Vector3> ringPoints = new List<Vector3>();

		for (float deg = 0; deg < 2 * Mathf.PI; deg += 2 * Mathf.PI / numPoints) {
			ringPoints.Add(orth[0] * Mathf.Sin(deg) + orth[1] * Mathf.Cos(deg));
		}


		for (int i = 0; i < numPoints; i++) {
			ringPoints[i] = ringPoints[i].normalized + normal * .3f;
			ringPoints[i] += point;
		}
		teleRing.positionCount = numPoints;
		teleRing.SetPositions(ringPoints.ToArray());
		teleRing.enabled = true;
	}
	
	//helper function that increments the current arc position and velocity vectors by one segment
	//call repeatedly to create an entire arc
	private bool getNextArcSegment(ref Vector3 pos, ref Vector3 velocity, float vScale, Vector3 accel, float timeStep) {
		Vector3 newPos;
		RaycastHit hit;

		//calculate new position
		newPos.x = (float)(pos.x + (velocity.x * timeStep) + (0.5 * accel.x * timeStep * timeStep));
		newPos.y = (float)(pos.y + (velocity.y * timeStep) + (0.5 * accel.y * timeStep * timeStep));
		newPos.z = (float)(pos.z + (velocity.z * timeStep) + (0.5 * accel.z * timeStep * timeStep));

		//raycast in that direction until an object is hit or we reach the new position
		Vector3 dir = newPos - pos;
		Physics.Raycast (pos, dir, out hit, dir.magnitude);


		if (hit.point != Vector3.zero) { //we've hit an object, this is the endpoint
			pos = hit.point;
			drawLine(hit.point, hit.normal);
			drawRing(hit.point, hit.normal, 12);
			return true;
		} else {
			pos = newPos;
		}
		velocity.x = velocity.x + accel.x * timeStep;
		velocity.y = velocity.y + accel.y * timeStep;
		velocity.z = velocity.z + accel.z * timeStep;
		return false;
	}

}
