using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorHandler : MonoBehaviour {

	public string otherSide = "level_1";

	bool isOpen = false;
	private int NUM_DOORS = 4;
	public GameObject[] doorObjects;
	public float openTime;

	private Vector3[] initialPos = {
		new Vector3(1, 1, 0),
		new Vector3(1, -1, 0),
		new Vector3(-1, 1, 0),
		new Vector3(-1, -1, 0)
	};

	private Vector3[] finalPos = {
		new Vector3(1, 3, 0),
		new Vector3(3, -1, 0),
		new Vector3(-3, 1, 0),
		new Vector3(-1, -3, 0)
	};


	// Use this for initialization
	void Start () {
		//SetOpen(true);
	}

	// Update is called once per frame
	bool status = false;
	void Update () {
		if (Input.GetKeyUp (KeyCode.R)) {
			status = !status;
			SetOpen(status);
		}
	}

	public void SetOpen(bool open) {
		if (isOpen != open) {
			if (isOpen) {
				StartCoroutine(Close());
			} else {
				StartCoroutine(Open());
			}
			isOpen = open;
		}
	}

	public IEnumerator Open() {
		float t = 0;
		while (t < 1) {
			t += Time.deltaTime/openTime;
			for (int i = 0; i < NUM_DOORS; i++) {
				doorObjects[i].transform.localPosition = Vector3.Lerp(initialPos[i], finalPos[i], t);
			}
			yield return null;
		}
		for (int i = 0; i < NUM_DOORS; i++) {
			doorObjects[i].transform.localPosition = finalPos[i];
			Debug.Log(finalPos[i]);
		}

	}

	public IEnumerator Close() {
		float t = 0;
		while (t < 1) {
			t += Time.deltaTime/openTime;
			for (int i = 0; i < NUM_DOORS; i++) {
				doorObjects[i].transform.localPosition = Vector3.Lerp(finalPos[i], initialPos[i], t);
			}
			yield return null;
		}
		for (int i = 0; i < NUM_DOORS; i++) {
			doorObjects[i].transform.localPosition = initialPos[i];
		}

	}


}
