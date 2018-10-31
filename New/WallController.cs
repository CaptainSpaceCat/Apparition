using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WallController : MonoBehaviour {
#if UNITY_EDITOR
	public bool[] windowTriangles;
	public MeshFilter mf;
	private bool[] currentTriangles;
	private Mesh mesh;

	void Start () {
		if (Application.isEditor && !Application.isPlaying)  {
			Reset();
		}
	}

	void Update() {
		if (Application.isEditor && !Application.isPlaying)  {
			Calculate();
		}
	}

	void Reset() {
		currentTriangles  = new bool[200];
		if (windowTriangles.Length != 200) {
			windowTriangles = new bool[200];
		}
		for (int i = 0; i < 200; i++) {
			currentTriangles[i] = windowTriangles[i];
		}
	}

	void Calculate() {
		//mesh = GetComponent<MeshFilter>().mesh;
		mesh = mf.sharedMesh;
		for (int i = 0; i < windowTriangles.Length; i++) {
			if (windowTriangles[i] != currentTriangles[i]) {
				mesh.triangles = flipTriangle(mesh.triangles, i);
				currentTriangles[i] = windowTriangles[i];
			}
		}

	}

	int[] flipTriangle(int[] triangles, int index) {
		index *= 3;
		int temp = triangles[index];
		triangles[index] = triangles[index + 2];
		triangles[index + 2] = temp;
		return triangles;
	}
#endif
}
