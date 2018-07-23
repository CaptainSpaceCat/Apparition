using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public enum ButtonState {DEACTIVATED, ACTIVATED, PREPARED};

	public float intensity = 1;
	public ButtonState state = ButtonState.DEACTIVATED;
	public float regHeight = .12f;
	public float preparedHeight = .25f;

	//TODO write transition animations and implement press in player controller
	//TODO Contains a method "isNear" that returns true if player is near.
	public GameObject player; //TODO Contains a method "isPrepared" that returns true if player is near AND pointing at this button.
	public float radius = 10f;

	void Update () {
		UpdateButtonState ();
	}

	void UpdateButtonState() {
		PlayerController controller = player.GetComponent<PlayerController> ();

		if (controller.isNear (transform.position, radius)) {
			if (controller.isPrepared (transform.position, radius)) {
				updateButtonProperties (state, ButtonState.PREPARED, preparedHeight);
			} else {
				updateButtonProperties (state, ButtonState.ACTIVATED, regHeight);
			}
		} else {
			updateButtonProperties (state, ButtonState.DEACTIVATED, regHeight);
		}
	}

	void updateButtonProperties(ButtonState previousState, ButtonState currentState, float height) {
		this.state = currentState;
		Vector3 scale = gameObject.transform.localScale;
		scale.y = height;
		gameObject.transform.localScale = scale; 

		//TODO get the glow to work
		if (previousState == ButtonState.PREPARED 
			&& (currentState == ButtonState.ACTIVATED || currentState == ButtonState.DEACTIVATED)) {
			DynamicGI.SetEmissive(GetComponent<Renderer>(), new Color(1f, 1f, 1f, 1.0f) * intensity);
		} else if (currentState == ButtonState.PREPARED 
			&& (previousState == ButtonState.ACTIVATED || previousState == ButtonState.DEACTIVATED)) {
			DynamicGI.SetEmissive(GetComponent<Renderer>(), new Color(1f, 1f, 1f, 0f) * intensity);
		}
	}

	public void pressButton() {
		//TODO cue animation here
	}
}