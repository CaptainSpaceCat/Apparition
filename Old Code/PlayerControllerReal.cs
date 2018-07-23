using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerReal : MonoBehaviour {

	Transform tForm;
	public float playerHeight = 2.0f;
	public float timePeriod = 5.0f;

	void Start () {
		tForm = gameObject.GetComponent<Transform>();
		Vector3 wallHit = new Vector3(50f,0f,50f);
		Vector3 lookDir = Vector3.left;
		Debug.Log("Wall Hit: ");
		Debug.Log(wallHit);
		Debug.Log("\nLook Dir: ");
		Debug.Log(lookDir);
		Vector3 fPos = wallHit + getOffset(lookDir, playerHeight);
		Debug.Log("\nFinal Pos: ");
		Debug.Log(fPos);
		StartCoroutine(Move(wallHit, lookDir, timePeriod));
	}
	//Smoothly translates and rotates player to new location in new direction in a length of time timePeriod
	public IEnumerator Move(Vector3 fPos, Vector3 lookDir, float timePeriod) {
		Vector3 iPos = tForm.position;
		Quaternion iRot = tForm.rotation;
		Quaternion fRot = Quaternion.LookRotation(lookDir);
		float t = 0.0f;
	
		while (t < 1) {
			t += Time.deltaTime / timePeriod;
			tForm.position = Vector3.Lerp(iPos, fPos, t);
			tForm.rotation = Quaternion.Lerp(iRot, fRot, t);
			yield return null;
		}

		tForm.position = fPos;
		tForm.rotation = fRot;

		Debug.Log(Time.time);
	}
	//Returns the offset due to the player height in the direction of lookDir
	private Vector3 getOffset(Vector3 lookDir, float playerHeight) {
		return lookDir.normalized * playerHeight;
	}
}
