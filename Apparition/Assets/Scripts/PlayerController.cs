using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{

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

	public Color col;
	public Texture2D tex;

	[RangeAttribute (1, 10)]
	public float sensitivity;

	void Start ()
	{
		rigidbody = GetComponent<Rigidbody> ();
		rigidbody.freezeRotation = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		FreeControls ();

		/*Vector3 s = new Vector3 (5, 5, 5);
		Vector3 f = new Vector3 (9, 9, 9);

		Vector3 t0 = new Vector3 (5, 6, 5);
		Vector3 tf = new Vector3 (9, 10, 9);*/

		//Handles.DrawBezier (s, f, t0, tf, Color.white, null, 2f);
	}

   

	void FixedUpdate ()
	{

		if (!controlsLocked) {

			//
			//rigidbody.position = hit.point + new Vector3(0, (float).5, 0);
		
			//}

			xDir += Input.GetAxis ("Mouse X") * sensitivity;
			yDir -= Input.GetAxis ("Mouse Y") * sensitivity;

			if (yDir > 90) {
				yDir = 90;
			} else if (yDir < -90) {
				yDir = -90;
			}

			rigidbody.rotation = Quaternion.Euler (yDir, xDir, 0.0f);
		}
	}

	void LockControls ()
	{
		controlsLocked = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void FreeControls ()
	{
		controlsLocked = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void ResetPosition ()
	{
		rigidbody.position = new Vector3 (-2, -2, -2);
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