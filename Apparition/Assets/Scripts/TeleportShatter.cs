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
			active = true;
			foreach (TriangleExplosion tri in children) {
				tri.activate ();
			}
			StartCoroutine (Rotate ());
			//StartCoroutine (spin (360, 2f));
		}
	}

	public IEnumerator Rotate() {


		Vector3 rot = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

		for (int i = 1; i <= 90; i++) {
			transform.Rotate (rot * 4);
			yield return new WaitForSeconds (.01f);
		}
		transform.rotation = initial;
		active = false;
		//transform.Rotate (Vector3.zero);
	}

	public IEnumerator spin(int degrees, float time) {
		Vector3 rot = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
		Quaternion oldRotation = transform.rotation;
		transform.Rotate (rot * degrees);
		Quaternion newRotation = transform.rotation;
		//add midrotation at 180 degrees, as oldrotation and newrotation should technically be the same value

		for (float t = 0.0f; t < time; t += Time.deltaTime) {
			transform.rotation = Quaternion.Slerp (oldRotation, newRotation, t);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		transform.rotation = newRotation;
		active = false;
	}

/*	public IEnumerator shatterChildren() {
		List<GameObject> shards = new List<GameObject> ();
		Transform[] childTransforms = GetComponentsInChildren<Transform> ();
		foreach (Transform t in childTransforms) {
			Debug.Log (t.position);
			Debug.Log (t.rotation);
		}
		MeshFilter[] childFilters = GetComponentsInChildren<MeshFilter> ();
		Mesh[] childMeshes = new Mesh[childFilters.Length];

		for (int i = 0; i < childFilters.Length; i++) { 
			childMeshes [i] = childFilters [i].mesh;
		}

		MeshRenderer[] childRenders = GetComponentsInChildren<MeshRenderer> ();
		//Material[] childMaterials = GetComponentsInChildren<Material> ();

		foreach (MeshRenderer mesh in childRenders) {
			mesh.enabled = false;
		}

		for (int m = 0; m < childMeshes.Length; m++) {
			Mesh mesh = childMeshes [m];
			Vector3[] verts = mesh.vertices;
			Vector3[] normals = mesh.normals;
			Vector2[] uvs = mesh.uv;
			for (int i = 0; i < mesh.subMeshCount; i++) {
				int[] indices = mesh.GetTriangles (i);

				for (int j = 0; j < indices.Length; j++) {
					
					Vector3[] newVerts = new Vector3[3];
					Vector3[] newNormals = new Vector3[3];
					Vector2[] newUvs = new Vector2[3];

					for (int k = 0; k < 3; k++) {
						
						int index = indices[i + k];

						newVerts[k] = verts[index];
						newNormals [k] = normals [index];
						newUvs [k] = uvs [index];
					}

					Mesh newMesh = new Mesh ();
					newMesh.vertices = newVerts;
					newMesh.normals = newNormals;
					newMesh.uv = newUvs;

					newMesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

					GameObject triangle = new GameObject ();
					shards.Add (triangle);
					triangle.transform.position = childTransforms [m].position;// - transform.position;
					triangle.transform.rotation = childTransforms [m].rotation; //+ transform.rotation;
					triangle.AddComponent<MeshRenderer> ();//.material = childMaterials [m];
					triangle.AddComponent<MeshFilter> ().mesh = newMesh;
				}
			}
		}

		for (int i = 0; i < 12; i++) {
			yield return new WaitForSeconds (1.0f);
		}

		foreach (GameObject g in shards) {
			Destroy (g);
		}

		foreach (MeshRenderer mesh in childRenders) {
			mesh.enabled = true;
		}
	}*/
}
