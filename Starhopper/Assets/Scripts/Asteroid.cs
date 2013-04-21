using UnityEngine;
using System.Collections;
[System.Serializable]
public class Asteroid {

	public long SpkID;
	public string FullName;
	public float SemiMajorAxis;
	public float Eccentricity;
	public float Inclination;
	public float AscNodeLongitude;
	public float ArgOfPerihelion;
	public float PerihelionDistance;
	public float AphelionDistance;
	public float PeriodYears;
	public float Magnitude;
	public float RotationalPeriod;
	public string SpecTypeSMASSII;
	public string SpecTypeTholen;
	public float MeanMotion;
	public float PeriodDays;
	public string OrbitClass;
	public string Producer;
	public string DateOfFirstObs;
	public float Diameter;
#if UNITY_EDITOR
	[UnityEditor.MenuItem("Asteroidz/Load")]
#endif
	static void Load()
	{
		Asteroid[] asteroids = LoadAsteroids.Load();
		asteroids = System.Array.FindAll(asteroids, a => a.Diameter > 0);
		System.Array.Sort(asteroids, (x, y) => -x.Eccentricity.CompareTo(y.Eccentricity));
		for (int i = 0; i < asteroids.Length; i++) {
			CreateEllipse(asteroids[i]);
			if (i > 100)
			              break;
		}
	}
	
	#if UNITY_EDITOR
	[UnityEditor.MenuItem("Asteroidz/Create Ellipse")]
	#endif
	static void CreateEllipse(Asteroid asteroid)
	{
		int ellipseSamples = 200;
		Vector3[] vertices = new Vector3[ellipseSamples];
		int[] triangles = new int[ellipseSamples];


		float sunDistanceFromCenter = (asteroid.AphelionDistance - asteroid.SemiMajorAxis);

		Quaternion rotation = Quaternion.Euler(asteroid.ArgOfPerihelion,asteroid.AscNodeLongitude,0);


		float angle = 0;
		float angleDelta = ((float)1 / (float)ellipseSamples) * Mathf.PI * 2;
		int lineIndex = 0;

		float semiMinorAxis = Mathf.Sqrt(-(Mathf.Pow(asteroid.Eccentricity, 2) * Mathf.Pow(asteroid.SemiMajorAxis, 2) - Mathf.Pow(asteroid.SemiMajorAxis, 2)));

		for (int i = 0; i < ellipseSamples; i++) {
			vertices[i] = new Vector3(Mathf.Cos (angle) * asteroid.SemiMajorAxis, Mathf.Sin(angle) * semiMinorAxis, 0);
			triangles[lineIndex++] = i;
			//if (i > 0)
				//triangles[lineIndex++] = i - 1;
			angle += angleDelta;
		}
		
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.SetIndices(triangles, MeshTopology.Lines, 0);
		mesh.name = "ellipseMesh";
		mesh.RecalculateBounds();
		
		GameObject go = new GameObject("ellipseMesh");
		go.transform.rotation = rotation;
		go.transform.position = go.transform.TransformPoint(new Vector3(1,0,0) * sunDistanceFromCenter);
		go.AddComponent<MeshFilter>();
		go.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = go.GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
		meshFilter.sharedMesh = mesh;

		GameObject asteroidMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		asteroidMesh.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f) * asteroid.Diameter;;
		GameObject asteroidObject = new GameObject("Asteroid orbit");
		asteroidMesh.transform.parent = asteroidObject.transform;
		AsteroidOrbit orbit = asteroidObject.AddComponent<AsteroidOrbit>();
		orbit.AsteroidYo = asteroid;
		
	}

}
