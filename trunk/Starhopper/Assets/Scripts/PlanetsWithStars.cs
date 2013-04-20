using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetsWithStars : MonoBehaviour
{
    private Star[] stars;
    private Dictionary<int, List<Exoplanet>> starExoplanets;

    // Use this for initialization
    void Start()
    {
        stars = LoadStars.Load();

        starExoplanets = LoadExoplanet.Load(stars);
        Debug.Log("Stars with planets = " + starExoplanets.Count);
    }

    void OnGUI()
    {

        foreach (KeyValuePair<int, List<Exoplanet>> entry in starExoplanets)
        {
            int starID = entry.Key;
            List<Exoplanet> planets = entry.Value;

            Star star = stars[starID]; // todo: make sure that its the correct star...

            Vector3 starPos = new Vector3(star.X, star.Y, star.Z) * Scaler.Scale;

            Vector3 screenPos3D = Camera.mainCamera.WorldToScreenPoint(starPos);
            screenPos3D.y = Screen.height - screenPos3D.y;

            if (screenPos3D.z < 0)
                continue;

            float distance = Vector3.Distance(starPos, Camera.main.transform.position);
            Vector2 screenPos = new Vector2(screenPos3D.x, screenPos3D.y);
            GUI.Label(new Rect(screenPos.x, screenPos.y, 200, 100), "Planets: " + planets.Count + " Dist: " + distance);
        }
    }
}
