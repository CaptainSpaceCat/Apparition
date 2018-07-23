using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

	public enum LeverState {OFF, ON};
	public Vector3 direction = new Vector3(-60, 0, 0);

	public LeverState state  = LeverState.OFF; //public for testing purposes.

	public void pressLever() {
		//TODO cue animation here

		if (state == LeverState.OFF) {
			state = LeverState.ON;
			transform.Rotate (direction);
		} else {
			state = LeverState.OFF;
			transform.Rotate (-direction);
		}
	}
}
