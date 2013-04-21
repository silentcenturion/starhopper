using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;


public class Sun : MonoBehaviour
{
		float _Rotation;
		public float RotationSpeed = 3;
		private float _ScaleSpeed = 0.2f;
		private float _Delay = 0;
		private float _DelayTimer = 0;
		public float Scale = 1;
		private Vector3 OriginalPos;
		public Star Star;
		Transform[] _Planets = new Transform[0];
		Exoplanet[] _PlanetData;
		GameObject[] _PlanetOrbits;

		// Use this for initialization
		void Start ()
		{
				_Rotation = 0;
				Scale = 0;
				_DelayTimer = 0;
		}

		// Update is called once per frame
		void Update ()
		{
				_Rotation += Time.deltaTime;
				Quaternion rot = Quaternion.Euler (_Rotation * RotationSpeed, 0f, 0f);
				Matrix4x4 m = Matrix4x4.TRS (Vector3.zero, rot, new Vector3 (1, 1, 1));

				Material material = GetComponent<MeshRenderer> ().sharedMaterial;
				material.SetMatrix ("_Rotation", m);
				material.SetFloat ("_Blend", Scale);

				_DelayTimer += Time.deltaTime;
				if (_DelayTimer > _Delay) {
						Scale += Time.deltaTime * _ScaleSpeed;
				}

				if (Scale > 1)
						Scale = 1;

				transform.localScale = Vector3.one * Scale;

				// Updat1e Orbit
				if (_Planets != null && _Planets.Length > 0) {
						for (int i = 0; i < _Planets.Length; i++) {
								Transform planet = _Planets [i];
								Exoplanet planetData = _PlanetData [i];
				
								float semiMinorAxis = Mathf.Sqrt (-(Mathf.Pow (planetData.OrbitalEccentricity, 2) * Mathf.Pow (planetData.SemiMajorAxis, 2) - Mathf.Pow (planetData.SemiMajorAxis, 2)));
				
								float angle = (Time.time * 356 / planetData.OrbitalPeriod) * Mathf.PI * 2;
								Vector3 position = new Vector3 (Mathf.Cos (angle) * planetData.SemiMajorAxis, Mathf.Sin (angle) * semiMinorAxis, 0);
								planet.localPosition = position;
						}
				}
        
				transform.position = OriginalPos * Scaler.Scale;
		}



#if UNITY_EDITOR
    [MenuItem("Space App Challenge/Create SolarSystem")]
    public static void CreateSolarSystem()
    {
        CreateSolarSystem(null);
    }
#endif

		public static void CreateSolarSystem (Star star)
		{
				Object[] solarSystems = Object.FindObjectsOfType (typeof(Sun));
				foreach (var solarSystem in solarSystems) {
						DestroyImmediate ((solarSystem as Sun).gameObject);
				}
        
        
				Sun sun = GenerateSun (star);
				sun.Star = star;
		}

		public static void RemoveSolarSystems ()
		{
				Object[] solarSystems = Object.FindObjectsOfType (typeof(Sun));
				foreach (var solarSystem in solarSystems) {
						DestroyImmediate ((solarSystem as Sun).gameObject);
				}

		}

		public void CreatePlanets (System.Collections.Generic.List<Exoplanet> planets)
		{
				Universe universe = Object.FindObjectOfType (typeof(Universe)) as Universe;

				_PlanetOrbits = new GameObject[planets.Count];
				_PlanetData = planets.ToArray ();
				_Planets = new Transform[planets.Count];
				for (int i = 0; i < planets.Count; i++) {
						Exoplanet planet = planets [i];
						GameObject planetParent = new GameObject ("planetParent");
						GameObject go = new GameObject ("planet");
						MeshFilter meshFilter = go.AddComponent<MeshFilter> ();
						MeshRenderer meshRenderer = go.AddComponent<MeshRenderer> ();
						Mesh mesh = GenerateMesh (Color.white);
						meshFilter.sharedMesh = mesh;
						meshRenderer.sharedMaterial = universe.PlanetMaterial;


						planetParent.transform.parent = transform;
						planetParent.transform.localPosition = Vector3.zero;
						go.transform.parent = planetParent.transform;
            
						float inclination = _PlanetData [i].Inclination;
						if (inclination == 0)
								inclination = Random.Range (80, 100);
						planetParent.transform.localRotation = Quaternion.Euler (0, inclination, 0);
						go.transform.localPosition = new Vector3 (planet.SemiMajorAxis, 0, 0);
						if (planet.PlanetaryRadius == 0)
								go.transform.localScale = Vector3.one * 0.1f;
						else
								go.transform.localScale = Vector3.one * 0.1f * planet.PlanetaryRadius;

						_Planets [i] = go.transform;
						_PlanetOrbits [i] = Orbit.CreateOrbitObject (Vector3.zero, planet.SemiMajorAxis, planet.SemiMajorAxis, 0, inclination, planet.OrbitalEccentricity, universe.AsteroidMaterial); 
						_PlanetOrbits [i].transform.parent = this.transform;
				}
		}

		static Sun GenerateSun (Star star)
		{
				GameObject go = new GameObject ("sun");
				Sun sun = go.AddComponent<Sun> ();
				MeshRenderer meshRenderer = go.AddComponent<MeshRenderer> ();


				Universe universe = Object.FindObjectOfType (typeof(Universe)) as Universe;
				meshRenderer.sharedMaterial = universe.SunMaterial;

				Color color = StarMesh.GetStarColor (star);

				Mesh mesh = GenerateMesh (color);
				MeshFilter meshFilter = go.AddComponent<MeshFilter> ();
				meshFilter.sharedMesh = mesh;

				sun.OriginalPos = new Vector3 (star.X, star.Y, star.Z);

                System.Collections.Generic.List<Exoplanet> planets = star.Planets;
                if (star.GetName() == "Sol")
                {
                    planets = GetOurPlanets();
                }

                if (planets != null && planets.Count > 0)
                {
                    sun.CreatePlanets(planets);
                }

                //if (star.Planets != null && star.Planets.Count > 0) {
                //        sun.CreatePlanets (star.Planets);
                //}

				sun.transform.localScale = Vector3.zero;

				return sun;
		}

        static System.Collections.Generic.List<Exoplanet> GetOurPlanets()
        {
            System.Collections.Generic.List<Exoplanet> planets = new System.Collections.Generic.List<Exoplanet>();

            Exoplanet merkurius = new Exoplanet();
            Exoplanet venus = new Exoplanet();
            Exoplanet earth = new Exoplanet();
            Exoplanet mars = new Exoplanet();
            Exoplanet jupiter = new Exoplanet();
            Exoplanet saturnus = new Exoplanet();
            Exoplanet uranus = new Exoplanet();
            Exoplanet neptunus = new Exoplanet();

            merkurius.PlanetaryRadius = 0.03413f;
            venus.PlanetaryRadius = 0.084639f;
            earth.PlanetaryRadius = 0.089213f;
            mars.PlanetaryRadius = 0.047516f;
            jupiter.PlanetaryRadius = 1f;
            saturnus.PlanetaryRadius = 0.843003f;
            uranus.PlanetaryRadius = 0.357509f;
            neptunus.PlanetaryRadius = 0.346388f;

            merkurius.SemiMajorAxis = 0.387098f;
            venus.SemiMajorAxis = 0.723327f;
            earth.SemiMajorAxis = 1f;
            mars.SemiMajorAxis = 1.523679f;
            jupiter.SemiMajorAxis = 5.204267f;
            saturnus.SemiMajorAxis = 9.58201720f;
            uranus.SemiMajorAxis = 19.22941195f;
            neptunus.SemiMajorAxis = 30f;

            merkurius.OrbitalPeriod = 39.264f;
            venus.OrbitalPeriod = 224.7f;
            earth.OrbitalPeriod = 365.2422f;
            mars.OrbitalPeriod = 686.98f;
            jupiter.OrbitalPeriod = 4331.77f;
            saturnus.OrbitalPeriod = 10760f;
            uranus.OrbitalPeriod = 30684f;
            neptunus.OrbitalPeriod = 60188.3f;

            planets.Add(merkurius);
            planets.Add(venus);
            planets.Add(earth);
            planets.Add(mars);
            planets.Add(jupiter);
            planets.Add(saturnus);
            planets.Add(uranus);
            planets.Add(neptunus);

            return planets;
        }
    
		static Mesh GenerateMesh (Color color)
		{
				Vector3[] vertices = new Vector3[4];
				Vector2[] uvs = new Vector2[4];
				int[] triangles = new int[6];
				Color[] colors = new Color[4];

				int vertIndex = 0;
				int uvIndex = 0;
				int triangleIndex = 0;

				int vert1 = vertIndex++;
				int vert2 = vertIndex++;
				int vert3 = vertIndex++;
				int vert4 = vertIndex++;

				//// Abs MAg
				//float absMag = stars[i + offset].AbsMag;
				//float normalizedMag = (absMag - minMag) / (maxMag - minMag);
				//color.a = 1 - normalizedMag;

				colors [vert1] = color;
				colors [vert2] = color;
				colors [vert3] = color;
				colors [vert4] = color;

				Vector3 position = Vector3.zero;
				vertices [vert1] = position;
				vertices [vert2] = position;
				vertices [vert3] = position;
				vertices [vert4] = position;

				uvs [uvIndex++] = new Vector2 (1, 1);
				uvs [uvIndex++] = new Vector2 (1, -1);
				uvs [uvIndex++] = new Vector2 (-1, -1);
				uvs [uvIndex++] = new Vector2 (-1, 1);

				triangles [triangleIndex++] = vert1;
				triangles [triangleIndex++] = vert2;
				triangles [triangleIndex++] = vert3;

				triangles [triangleIndex++] = vert3;
				triangles [triangleIndex++] = vert4;
				triangles [triangleIndex++] = vert1;


				Mesh mesh = new Mesh ();
				mesh.vertices = vertices;
				mesh.uv = uvs;
				mesh.colors = colors;
				mesh.name = "sunOrPlanet";
				mesh.triangles = triangles;
				mesh.RecalculateBounds ();
        
				return mesh;
		}
}
