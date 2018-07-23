using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public enum MovingDirection{TO_START, TO_END};

	public float totalTime = 10;
	public Vector3 start = new Vector3(-2, 2, 7);
	public Vector3 end = new Vector3(-2, 7, 7);

	private Coroutine moving;
	private float totalMag;
	public MovingDirection state = MovingDirection.TO_START;

	public void Start() {
		totalMag = (end - start).magnitude;
		transform.position = start;
	}

	public void ChangeDirection() {
		if (moving != null) {
			StopCoroutine (moving);
		}
			
		Vector3 currentEnd;
		float seconds;

		if (state == MovingDirection.TO_START) {
			state = MovingDirection.TO_END;
			currentEnd = this.end;
		} else {
			state = MovingDirection.TO_START;
			currentEnd = this.start;
		}

		moving = StartCoroutine(Move(transform.position, currentEnd, totalTime * (currentEnd - transform.position).magnitude / totalMag));
	}

	IEnumerator Move(Vector3 start, Vector3 end, float time) {
		float step = 1f/time * Time.deltaTime;
		float t = 0;
		while (t <= 1.0f) {
			t += step;
			transform.position = Vector3.Lerp(start, end, t);
			yield return new WaitForFixedUpdate();
		}
		transform.position = end;
	}
}