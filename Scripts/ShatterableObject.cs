using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterableObject {

	private Vector3 initialPosition;
	private Quaternion initialRotation;
	public GameObject mainObject;

	private ShatterTriangle[] triangles;
	private Vector3 motionVector;


	//constructor, cuts this object into triangles and prepares it to be shattered when needed
	public ShatterableObject(GameObject g) {
		mainObject = g;
		triangles = splitMesh (g);
		setTrianglesEnabled (false);
		newMotionVector();

		//set initial transforms, so that this object can be reset after a teleport
		initialPosition = mainObject.transform.position;
		initialRotation = mainObject.transform.rotation;
	}

	//helper function that sets all of this objects triangles either rendered or not
	//sets this object's enabled status to the opposite of the status of the triangles
	//so that only one of the two is ever rendered
	public void setTrianglesEnabled(bool enabled) {
		mainObject.GetComponent<MeshRenderer> ().enabled = !enabled;
		foreach (ShatterTriangle tri in triangles) {
			tri.setEnabled(enabled);
		}
	}

	//helper function that resets all of the transforms for this object to their defaults
	//used after a teleport to make sure the room stays the way it was
	public void resetTransform() {
		mainObject.transform.position = initialPosition;
		mainObject.transform.rotation = initialRotation;
		for (int i = 0; i < triangles.Length; i++) {
			triangles[i].resetTransform();
		}
		newMotionVector();
	}

	//sets up a new random direction of rotation for shattering
	public void newMotionVector() {
		motionVector = Random.onUnitSphere;
	}

	//sets this object to the correct rotational position from 0 to 1,
	//both 0 and 1 being its normal position and everything in between being a 360 spin
	//synchronizes with children
	public void setInterpolatedTransform(float t, float d) {
		foreach (ShatterTriangle tri in triangles) {
			tri.setInterpolatedTransform(t, d);
		}
		mainObject.transform.rotation = initialRotation * Quaternion.AngleAxis(t*360, motionVector);
	}




	//complex helper function, used once, that splits this object's mesh into triangles
	private ShatterTriangle[] splitMesh(GameObject parent) {
		List<ShatterTriangle> triangles = new List<ShatterTriangle> ();

		Mesh M = new Mesh();
		if(parent.GetComponent<MeshFilter>()) {
			M = parent.GetComponent<MeshFilter>().mesh;
		}
		else if(parent.GetComponent<SkinnedMeshRenderer>()) {
			M = parent.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		}

		Material[] materials = new Material[0];
		if(parent.GetComponent<MeshRenderer>()) {
			materials = parent.GetComponent<MeshRenderer>().materials;
		}
		else if(parent.GetComponent<SkinnedMeshRenderer>()) {
			materials = parent.GetComponent<SkinnedMeshRenderer>().materials;
		}

		Vector3[] verts = M.vertices;
		Vector3[] normals = M.normals;
		Vector2[] uvs = M.uv;
		for (int submesh = 0; submesh < M.subMeshCount; submesh++) {

			int[] indices = M.GetTriangles (submesh);

			for (int i = 0; i < indices.Length; i += 3) {
				Vector3[] newVerts = new Vector3[3];
				Vector3[] newNormals = new Vector3[3];
				Vector2[] newUvs = new Vector2[3];
				for (int n = 0; n < 3; n++) {
					int index = indices [i + n];
					newVerts [n] = verts [index];
					newUvs [n] = uvs [index];
					newNormals [n] = normals [index];
				}

				Mesh mesh = new Mesh ();
				mesh.vertices = newVerts;
				mesh.normals = newNormals;
				mesh.uv = newUvs;

				//mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };
				mesh.triangles = new int[] { 0, 1, 2 };

				GameObject GO = new GameObject ("Triangle " + (i / 3));
				//GO.layer = LayerMask.NameToLayer("Particle");
				GO.transform.position = mainObject.transform.position;
				GO.transform.rotation = mainObject.transform.rotation;
				GO.transform.localScale = mainObject.transform.localScale;
				GO.transform.SetParent (mainObject.transform);
				GO.AddComponent<MeshRenderer> ().material = materials [submesh];
				GO.AddComponent<MeshFilter> ().mesh = mesh;
				triangles.Add (new ShatterTriangle(GO));
			}
		}
		return triangles.ToArray ();
	}
}
