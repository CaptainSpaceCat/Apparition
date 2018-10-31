using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour {

	public RoomShatter roomShatter;
	public

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void Teleport() {
		roomShatter.activateShatter();
	}
}
