using UnityEngine;
using System.Collections;
using System;

public static class LoadAsteroids {
	
	public static Asteroid[] Load()
	{
		TextAsset starDatabase = (TextAsset)Resources.Load("results");

		string[] lines = starDatabase.text.Split('\r');
		int numAsteroids = lines.Length - 1;
		Debug.Log("Parsing " + numAsteroids + " asteroids...");
		Asteroid[] asteroids = new Asteroid[numAsteroids];

		int currentAsteroid = 0;
		for (int lineIndex = 1; lineIndex < lines.Length - 1; lineIndex++) // skip first line that contains headers
		{
			asteroids[currentAsteroid++] = ParseAsteroid(lines[lineIndex]);
			
			if (currentAsteroid > asteroids.Length)
				break;
		}
		Debug.Log(currentAsteroid + " asteroids parsed!");
		Array.Resize(ref asteroids, currentAsteroid);
		return asteroids;
	}
	private static Asteroid ParseAsteroid(string line)
	{
		Asteroid asteroid = new Asteroid();
		int i = 0;
		asteroid.SpkID = CSVUtils.ParseInt(line, ref i);
		asteroid.FullName = CSVUtils.ParseString(line, ref i);
		asteroid.SemiMajorAxis = CSVUtils.ParseFloat(line, ref i);
		asteroid.Eccentricity = CSVUtils.ParseFloat(line, ref i);
		asteroid.Inclination = CSVUtils.ParseFloat(line, ref i);
		asteroid.AscNodeLongitude = CSVUtils.ParseFloat(line, ref i);
		asteroid.ArgOfPerihelion = CSVUtils.ParseFloat(line, ref i);
		asteroid.PerihelionDistance = CSVUtils.ParseFloat(line, ref i);
		asteroid.AphelionDistance = CSVUtils.ParseFloat(line, ref i);
		asteroid.PeriodYears = CSVUtils.ParseFloat(line, ref i);
		asteroid.Magnitude = CSVUtils.ParseFloat(line, ref i);
		asteroid.RotationalPeriod = CSVUtils.ParseFloat(line, ref i);
		asteroid.SpecTypeSMASSII = CSVUtils.ParseString(line, ref i);
		asteroid.SpecTypeTholen = CSVUtils.ParseString(line, ref i);
		asteroid.MeanMotion = CSVUtils.ParseFloat(line, ref i);
		asteroid.PeriodDays = CSVUtils.ParseFloat(line, ref i);
		asteroid.OrbitClass = CSVUtils.ParseString(line, ref i);
		asteroid.Producer = CSVUtils.ParseString(line, ref i);
		asteroid.DateOfFirstObs = CSVUtils.ParseString(line, ref i);
		asteroid.Diameter = CSVUtils.ParseFloat(line, ref i);
		return asteroid;
	}
}