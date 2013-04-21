using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidVoyeur : MonoBehaviour {

	public Sun Sun;
	public Universe Universe;
	Asteroid[] _AllAsteroids;
	List<Asteroid> _AsteroidsByDiameter;
	List<Asteroid> _AsteroidsByEccentricity;
	List<Asteroid> _AsteroidsByPeriod;
	Dictionary<string, List<Asteroid>> _AsteroidsByOrbitClass = new Dictionary<string, List<Asteroid>>();
	float _Scale = 1;

    public static AsteroidVoyeur Instance;

    public List<GameObject> AsteroidRepresentations;


	void Start () {

        Instance = this;

		Universe = Object.FindObjectOfType(typeof(Universe)) as Universe;

		_AllAsteroids = LoadAsteroids.Load();
		_AsteroidsByDiameter = new List<Asteroid>(_AllAsteroids);
		_AsteroidsByDiameter.Sort((x, y) => -x.Diameter.CompareTo(y.Diameter));
		_AsteroidsByEccentricity = new List<Asteroid>(_AllAsteroids);
		_AsteroidsByEccentricity.Sort((x, y) => -x.Eccentricity.CompareTo(y.Eccentricity));
		_AsteroidsByPeriod = new List<Asteroid>(_AllAsteroids);
		_AsteroidsByPeriod.Sort((x, y) => x.PeriodYears.CompareTo(y.PeriodYears));
		for (int i = 0; i < _AllAsteroids.Length; i++) 
		{
			string oc = _AllAsteroids[i].OrbitClass;
			if (!_AsteroidsByOrbitClass.ContainsKey(oc))
				_AsteroidsByOrbitClass.Add(oc, new List<Asteroid>());
			_AsteroidsByOrbitClass[oc].Add(_AllAsteroids[i]);
		}

        AsteroidRepresentations = new List<GameObject>();
		for (int i = 0; i < 15; i++) {
			AsteroidRepresentations.Add(Asteroid.CreateRepresentation(_AsteroidsByDiameter[i], Universe.AsteroidMaterial));
		}
	}

    public void TopDiameter()
    {
        foreach (GameObject obj in AsteroidRepresentations)
            Destroy(obj);
        AsteroidRepresentations.Clear();
        for (int i = 0; i < 15; i++)
        {
            AsteroidRepresentations.Add(Asteroid.CreateRepresentation(_AsteroidsByDiameter[i], Universe.AsteroidMaterial));
        }
    }

    public void TopEccentricity()
    {
        foreach (GameObject obj in AsteroidRepresentations)
            Destroy(obj);
        AsteroidRepresentations.Clear();
        for (int i = 0; i < 15; i++)
        {
            AsteroidRepresentations.Add(Asteroid.CreateRepresentation(_AsteroidsByEccentricity[i], Universe.AsteroidMaterial));
        }
    }

    public void TopPeriod()
    {
        foreach (GameObject obj in AsteroidRepresentations)
            Destroy(obj);
        AsteroidRepresentations.Clear();
        for (int i = 0; i < 15; i++)
        {
            AsteroidRepresentations.Add(Asteroid.CreateRepresentation(_AsteroidsByPeriod[i], Universe.AsteroidMaterial));
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Y))
			_Scale += 0.2f;
		if (Input.GetKeyDown(KeyCode.U))
			_Scale -= 0.2f;
		float sunScale = 0f;
		if (!Sun)
			Sun = Object.FindObjectOfType(typeof(Sun)) as Sun;
		if (Sun && Sun.Star.GetName() == "Sol")
			sunScale = Sun.Scale;
		foreach (var ar in AsteroidRepresentations) {
				ar.transform.localScale = new Vector3(_Scale, _Scale, _Scale) * sunScale;
		}
	}
}
