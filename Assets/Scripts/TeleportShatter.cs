using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportShatter : MonoBehaviour {

	Quaternion initial;
	TriangleExplosion[] children;
	// Use this for initialization
	void Start () {
		initial = transform.rotation;
		children = GetComponentsInChildren<TriangleExplosion> ();
	}

	// Update is called once per frame
	bool active = false;
	void Update () {
		if (Input.GetKeyUp (KeyCode.Space) && !active) {
			//activate ();
			//StartCoroutine (spin (360, 2f));
		}
	}

	public void activate() {
		active = true;
		foreach (TriangleExplosion tri in children) {
			tri.activate ();
		}
		StartCoroutine (Rotate ());
	}

	public IEnumerator Rotate() {


		Vector3 rot = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

		for (int i = 1; i <= 90; i++) {
			transform.Rotate (rot * 4);
			yield return null;
		}
		transform.rotation = initial;
		active = false;
		//transform.Rotate (Vector3.zero);
	}
}