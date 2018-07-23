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
		StartCoroutine (flowMesh (triangles, 2f));
	}

	struct MotionVector {
		public Vector3 rotation;
		public Quaternion initial;

		public Vector3 iPos;
		public Vector3 fPos;
	}

	private float scale = 30;

	public IEnumerator flowMesh(List<GameObject> triangles, float timePeriod) {
		GetComponent<MeshRenderer> ().enabled = false;

		//yield return new WaitForSeconds (2f);

		MotionVector[] instructions = new MotionVector[triangles.Count];
		int n = 0;
		foreach (GameObject tri in triangles) {
			instructions [n].initial = tri.transform.rotation;
			instructions [n].rotation = Random.onUnitSphere;

			instructions [n].iPos = tri.transform.position;
			instructions [n].fPos = instructions [n].iPos + instructions [n].rotation * scale;
			n++;
		}

		Vector3 rot = Random.onUnitSphere;
		float t = 0.0f;
		while (t < 1) {
			t += Time.deltaTime/timePeriod;
			//transform.rotation = initial;
			//transform.Rotate (rot * t * 360);
			transform.rotation = initial * Quaternion.AngleAxis(t*360f, rot);
			//transform.Rotate (rot * 4);


			int c = 0;
			foreach (GameObject tri in triangles) {

				//tri.transform.Rotate (instructions [c].rotation * 4);
				tri.transform.rotation = instructions[c].initial * Quaternion.AngleAxis(t*360f, instructions[c].rotation);
				if (t < .5) {
					//tri.transform.Translate (instructions[c].rotation);
					tri.transform.position = Vector3.Lerp(instructions[c].iPos, instructions[c].fPos, t);
				} else {
					//tri.transform.Translate (-instructions[c].rotation);
					tri.transform.position = Vector3.Lerp(instructions[c].fPos, instructions[c].iPos, t);
				}

				c++;

			}
			yield return null;
		}

		GetComponent<MeshRenderer> ().enabled = true;
		if (GetComponent<Collider> ()) {
			GetComponent<Collider> ().enabled = true;
		}
		foreach (GameObject tri in triangles) {
			Destroy (tri);
		}
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
