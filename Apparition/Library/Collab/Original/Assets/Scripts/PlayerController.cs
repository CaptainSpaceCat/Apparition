using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour {

	//############ NOTE #############
	//when teleporting, if the new gravity is the same as the old gravity,
	//end up looking towards the same place as before
	//if the new gravity is drastically different though, 
	//look towards previous position
	//this way we can avoid disorientation when teleporting to a new surface
	//but also avoid annoyance if the player simply wants to take a step forward

	//wait new idea for when the new gravity is drastically different
	//we cant control where the player looks when teleporting to a new surface
	//because this is in VR, and the player chooses where to look based on head orientation
	//instead, we could just choose randomly or just not alter the way the camera was
	//or we could have the user press and hold to begin teleporting,
	//then drag a bit to dictate the direction they wish to end up looking towards
	//a little line would come out from the point of teleportation towards where
	//theyre dragging, so that they can see and choose where to look


    private float xDir = 0;
    private float yDir = 0;
    private new Rigidbody rigidbody;
    private bool controlsLocked = true;
	Vector3[] arcPos;

    public LineRenderer teleArc;
	public LineRenderer teleSpot;
	//public LineRenderer teleRing;

	public Color col;
	public Texture2D tex;

    [RangeAttribute(1, 10)]
    public float sensitivity;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FreeControls();

		/*Vector3 s = new Vector3 (5, 5, 5);
		Vector3 f = new Vector3 (9, 9, 9);

		Vector3 t0 = new Vector3 (5, 6, 5);
		Vector3 tf = new Vector3 (9, 10, 9);*/

		//Handles.DrawBezier (s, f, t0, tf, Color.white, null, 2f);
    }

    bool inAir() {
        RaycastHit hit;
        Physics.Raycast(rigidbody.transform.position, Vector3.down, out hit);
        if (hit.distance > .5) {
            return true;
        }
        return false;
    }

    void drawLine(Vector3[] hit) {
		
        /*if (hit == Vector3.zero) {
            teleArc.enabled = false;
        } else {*/
            /*Vector3[] arc = {
                rigidbody.position + transform.right * (float).2 + transform.up * (float)-.2,
                hit,
            };*/
            teleArc.SetPositions(hit);
            teleArc.enabled = true;
			
		
        //}
    }

void drawRing(RaycastHit hit, float scale, float offset) {
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
	RaycastHit getGunArc(Vector3 pos, Vector3 velocity, float vScale, Vector3 accel) {
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
			Physics.Raycast(pos, dir, out hit, dir.magnitude);

			if (hit.point != Vector3.zero) {
				arcPos [count] = pos;
				count++;
				//arcPos [count + 1] = hit.point;

				for (int i = count; i < MAX_SEGMENTS; i++) {
					arcPos [i] = hit.point;
				}
				teleArc.positionCount = MAX_SEGMENTS;
				drawLine(arcPos);

				Vector3[] spot = new Vector3[2];
				spot [0] = hit.point;
				spot [1] = spot [0] + hit.normal*3.5f;
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
		return new RaycastHit();
	}


    public float gunVelocity;

public Vector3 gravity = new Vector3(0f, -9.81f, 0f);

    void FixedUpdate() {

        if (!controlsLocked) {

            //
                //rigidbody.position = hit.point + new Vector3(0, (float).5, 0);

		RaycastHit hit = getGunArc(rigidbody.transform.position, rigidbody.transform.forward, 12f, gravity);
			if (Input.GetMouseButtonUp (0)) {
			rigidbody.position = hit.point;
				//gravity = hit.normal * -9.81f;
			}
            //}

            xDir += Input.GetAxis("Mouse X") * sensitivity;
            yDir -= Input.GetAxis("Mouse Y") * sensitivity;

            if (yDir > 90) {
                yDir = 90;
            } else if (yDir < -90) {
                yDir = -90;
            }

            rigidbody.rotation = Quaternion.Euler(yDir, xDir, 0.0f);
        }
    }

    void LockControls() {
        controlsLocked = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void FreeControls() {
        controlsLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ResetPosition() {
        rigidbody.position = new Vector3(-2, -2, -2);
        xDir = 0;
        yDir = 0;
    }


	/*void OnDrawGizmos() {
		for (int i = 1; i < 25; i++) {
			//Gizmos.DrawLine(arcPos[i-1], arcPos[i]);
			Gizmos.DrawCube(arcPos[i], Vector3.one/10);
		}
	}*/
}