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
	public static GameObject CreateRepresentation(Asteroid asteroid, Material mat)
	{

		GameObject go = Orbit.CreateOrbitObject(Vector3.zero, asteroid.AphelionDistance, asteroid.SemiMajorAxis, asteroid.ArgOfPerihelion, asteroid.AscNodeLongitude, asteroid.Eccentricity, mat);

		GameObject parentObj = new GameObject("asteroid " + asteroid.FullName);
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
