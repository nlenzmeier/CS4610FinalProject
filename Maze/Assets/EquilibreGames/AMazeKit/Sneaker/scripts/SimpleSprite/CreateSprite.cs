using UnityEngine;
using System.Collections;

public class CreateSprite {

	public static Mesh CreateTriangle() {
		Mesh simplePlane = new Mesh();
		simplePlane.Clear();

		// Four points of plane
		Vector3 p0 = new Vector3(0, 0, 0);
		Vector3 p1 = new Vector3(1, 0, 0);
		Vector3 p2 = new Vector3(0, 1, 0);
		//Vector3 p3 = new Vector3(1, 1, 0);
		// p2 - p3 
		// |  \ |
		// p0 - p1

		// Vertices
		simplePlane.vertices = new Vector3[] { p0, p1, p2 };

		// Triangles : 
		// CW (p0, p1, p2) (p1, p3, p2)
		//simplePlane.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
		// CCW (p0, p2, p1) (p2, p3, p1)
		//simplePlane.triangles = new int[] { 0, 2, 1, 2, 3, 1 };
		simplePlane.triangles = new int[] { 0, 2, 1};

		// UVs
		/*
		simplePlane.uv = new Vector2[] {
			new Vector2(1, 1),
			new Vector2(0, 1),
			new Vector2(1, 0),
			new Vector2(0, 0)};
			// */
		simplePlane.uv = new Vector2[] {
			new Vector2(0, 0),
			new Vector2(1, 0),
			new Vector2(0, 1)
			//,new Vector2(1, 1)
		};

		simplePlane.RecalculateNormals();
		simplePlane.RecalculateBounds();
		;

		return simplePlane;
	}
	
}
