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
		for (int i = 0; i < asteroids.Length; i++) {
			//CreateRepresentation(asteroids[i]);
			if (i > 100)
			              break;
		}
	}
	public static Color ColorFromHSV(float h, float s, float v, float a = 1)
	{
		// no saturation, we can return the value across the board (grayscale)
		if (s == 0)
			return new Color(v, v, v, a);
		
		// which chunk of the rainbow are we in?
		float sector = h / 60;
		
		// split across the decimal (ie 3.87 into 3 and 0.87)
		int i = (int)sector;
		float f = sector - i;
		
		float p = v * (1 - s);
		float q = v * (1 - s * f);
		float t = v * (1 - s * (1 - f));
		
		// build our rgb color
		Color color = new Color(0, 0, 0, a);
		
		switch(i)
		{
		case 0:
			color.r = v;
			color.g = t;
			color.b = p;
			break;
			
		case 1:
			color.r = q;
			color.g = v;
			color.b = p;
			break;
			
		case 2:
			color.r  = p;
			color.g  = v;
			color.b  = t;
			break;
			
		case 3:
			color.r  = p;
			color.g  = q;
			color.b  = v;
			break;
			
		case 4:
			color.r  = t;
			color.g  = p;
			color.b  = v;
			break;
			
		default:
			color.r  = v;
			color.g  = p;
			color.b  = q;
			break;
		}
		
		return color;
	}
	
	public static void ColorToHSV(Color color, out float h, out float s, out float v)
	{
		float min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
		float max = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
		float delta = max - min;
		
		// value is our max color
		v = max;
		
		// saturation is percent of max
		if (!Mathf.Approximately(max, 0))
			s = delta / max;
		else
		{
			// all colors are zero, no saturation and hue is undefined
			s = 0;
			h = -1;
			return;
		}
		
		// grayscale image if min and max are the same
		if (Mathf.Approximately(min, max))
		{
			v = max;
			s = 0;
			h = -1;
			return;
		}
		
		// hue depends which color is max (this creates a rainbow effect)
		if (color.r == max)
			h = (color.g - color.b) / delta;            // between yellow & magenta
		else if (color.g == max)
			h = 2 + (color.b - color.r) / delta;                // between cyan & yellow
		else
			h = 4 + (color.r - color.g) / delta;                // between magenta & cyan
		
		// turn hue into 0-360 degrees
		h *= 60;
		if (h < 0 )
			h += 360;
	}
	public static GameObject CreateRepresentation(Asteroid asteroid, Material mat)
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

		GameObject parentObj = new GameObject("asteroid " + asteroid.FullName);
		GameObject go = new GameObject("ellipseMesh");
		go.transform.rotation = rotation;
		go.transform.position = go.transform.TransformPoint(new Vector3(1,0,0) * sunDistanceFromCenter);
		go.AddComponent<MeshFilter>();
		go.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = go.GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
		meshFilter.sharedMesh = mesh;
		meshRenderer.sharedMaterial = new Material(mat);
		float h;
		float s;
		float v;
		ColorToHSV(mat.color, out h, out s, out v);
		h = Mathf.Repeat(h + Random.Range(0, 360), 360);
		Debug.Log(h);
		meshRenderer.sharedMaterial.SetColor("_Color", ColorFromHSV(h, s, v));


		GameObject asteroidMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		asteroidMesh.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f) * asteroid.Diameter;;
		GameObject asteroidObject = new GameObject("Asteroid orbit");
		asteroidMesh.transform.parent = asteroidObject.transform;
		AsteroidOrbit orbit = asteroidObject.AddComponent<AsteroidOrbit>();
		orbit.AsteroidYo = asteroid;

		go.transform.parent = parentObj.transform;
		asteroidObject.transform.parent = parentObj.transform;
		return parentObj;
		
	}

}
