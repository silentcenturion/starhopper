using UnityEngine;
using System.Collections;

public class AsteroidOrbit : MonoBehaviour {

	public Asteroid AsteroidYo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		float sunDistanceFromCenter = (AsteroidYo.AphelionDistance - AsteroidYo.SemiMajorAxis);
		
		Quaternion rotation = Quaternion.Euler(AsteroidYo.ArgOfPerihelion,AsteroidYo.AscNodeLongitude,0);
		
		
		float semiMinorAxis = Mathf.Sqrt(-(Mathf.Pow(AsteroidYo.Eccentricity, 2) * Mathf.Pow(AsteroidYo.SemiMajorAxis, 2) - Mathf.Pow(AsteroidYo.SemiMajorAxis, 2)));

		float angle = Time.time / AsteroidYo.PeriodYears * Mathf.PI * 2;
		Vector3 position = new Vector3(Mathf.Cos (angle) * AsteroidYo.SemiMajorAxis, Mathf.Sin(angle) * semiMinorAxis, 0);


		this.transform.rotation = rotation;
		this.transform.position = Vector3.zero;
		Vector3 offset = this.transform.TransformPoint(new Vector3(1,0,0) * sunDistanceFromCenter);
		position = this.transform.TransformPoint(position);
		this.transform.rotation = Quaternion.Euler(new Vector3(AsteroidYo.RotationalPeriod * Time.time,0,0));
		this.transform.position = position + offset;
	}
}
