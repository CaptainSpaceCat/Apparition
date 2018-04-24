using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TriangleExplosion : MonoBehaviour {

	List<GameObject> triangles;
	Quaternion initial;

	void Start() {
		initial = transform.rotation;


	}

	void Update() {
		
	}

	public void activate() {
		triangles = SplitMesh();
		StartCoroutine (flowMesh (triangles));
	}

	struct MotionVector {
		public Vector3 translation;
		public Vector3 rotation;
	}

	private float scale = .075f * 4;

	public IEnumerator flowMesh(List<GameObject> triangles) {
		GetComponent<MeshRenderer> ().enabled = false;

		//yield return new WaitForSeconds (2f);

		MotionVector[] instructions = new MotionVector[triangles.Count];
		for (int n = 0; n < triangles.Count; n++) {
			//instructions [n].translation = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
			instructions [n].rotation = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
			instructions [n].translation = instructions [n].rotation * scale;
		}
		Vector3 rot = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
		for (int i = 0; i < 90; i++) {
			transform.Rotate (rot * 4);


			int c = 0;
			foreach (GameObject tri in triangles) {

				tri.transform.Rotate (instructions [c].rotation * 4);
				if (i < 45) {
					tri.transform.Translate (instructions[c].translation);
				} else {
					tri.transform.Translate (instructions[c].translation * -1f);
				}

				c++;

			}
			yield return new WaitForSeconds (.0001f);
		}

		GetComponent<MeshRenderer> ().enabled = true;
		foreach (GameObject tri in triangles) {
			Destroy (tri);
		}
		transform.rotation = initial;
		yield return new WaitForSeconds (.05f);
		transform.rotation = initial;

	}

	public List<GameObject> SplitMesh ()    {
		List<GameObject> triangles = new List<GameObject> ();
		if(GetComponent<MeshFilter>() == null || GetComponent<SkinnedMeshRenderer>() == null) {
			//yield return null;
		}

		if(GetComponent<Collider>()) {
			GetComponent<Collider>().enabled = false;
		}

		Mesh M = new Mesh();
		if(GetComponent<MeshFilter>()) {
			M = GetComponent<MeshFilter>().mesh;
		}
		else if(GetComponent<SkinnedMeshRenderer>()) {
			M = GetComponent<SkinnedMeshRenderer>().sharedMesh;
		}

		Material[] materials = new Material[0];
		if(GetComponent<MeshRenderer>()) {
			materials = GetComponent<MeshRenderer>().materials;
		}
		else if(GetComponent<SkinnedMeshRenderer>()) {
			materials = GetComponent<SkinnedMeshRenderer>().materials;
		}

		Vector3[] verts = M.vertices;
		Vector3[] normals = M.normals;
		Vector2[] uvs = M.uv;
		for (int submesh = 0; submesh < M.subMeshCount; submesh++) {

			int[] indices = M.GetTriangles(submesh);

			for (int i = 0; i < indices.Length; i += 3)    {
				Vector3[] newVerts = new Vector3[3];
				Vector3[] newNormals = new Vector3[3];
				Vector2[] newUvs = new Vector2[3];
				for (int n = 0; n < 3; n++)    {
					int index = indices[i + n];
					newVerts[n] = verts[index];
					newUvs[n] = uvs[index];
					newNormals[n] = normals[index];
				}

				Mesh mesh = new Mesh();
				mesh.vertices = newVerts;
				mesh.normals = newNormals;
				mesh.uv = newUvs;

				//mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };
				mesh.triangles = new int[] {0, 1, 2};

				GameObject GO = new GameObject("Triangle " + (i / 3));
				//GO.layer = LayerMask.NameToLayer("Particle");
				GO.transform.position = transform.position;
				GO.transform.rotation = transform.rotation;
				GO.transform.localScale = transform.localScale;
				GO.transform.SetParent (transform);
				GO.AddComponent<MeshRenderer>().material = materials[submesh];
				GO.AddComponent<MeshFilter>().mesh = mesh;
				triangles.Add (GO);
				//GO.AddComponent<BoxCollider>();
				//Vector3 explosionPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(0f, 0.5f), transform.position.z + Random.Range(-0.5f, 0.5f));
				//GO.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(300,500), explosionPos, 5);
				//Destroy(GO, 5 + Random.Range(0.0f, 5.0f));
			}
		}



		return triangles;
	}


}