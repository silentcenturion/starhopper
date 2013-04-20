using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetsWithStars : MonoBehaviour
{
	public GUIManager guiManager;

    // Use this for initialization
    void Start()
    {
        LoadExoplanet.Load(StarPicker.Stars);
    }

    void OnGUI()
    {
		if( guiManager.ShowPlanets() )
		{
			GUIStyle planetStyle = new GUIStyle();
            Vector3 camPos = Camera.mainCamera.transform.position;
			
	        foreach (Star star in StarPicker.Stars)
	        {
                List<Exoplanet> planets = star.Planets;
	
	            Vector3 starPos = new Vector3(star.X, star.Y, star.Z) * Scaler.Scale;
	            float distance3D = Vector3.SqrMagnitude(camPos - starPos);
				
	            if (distance3D < guiManager.GetPlanetFilter() * guiManager.GetPlanetFilter() * Scaler.Scale)
	            {
	                Vector3 screenPos3D = Camera.mainCamera.WorldToScreenPoint(starPos);
	                screenPos3D.y = Screen.height - screenPos3D.y;
	
	                if (screenPos3D.z < 0)
	                    continue;
	
					
	                float distance = Vector3.Distance(starPos, camPos);
	                Vector2 screenPos = new Vector2(screenPos3D.x, screenPos3D.y);
					
					planetStyle.normal.textColor = new Color(255,255,255, 1 - (distance / guiManager.GetPlanetFilter()));
					
					if( planets.Count > 0 )
                		GUI.Label(new Rect(screenPos.x, screenPos.y, 200, 100), star.GetName()+"(Planets: " + planets.Count+")", planetStyle);
					else
                		GUI.Label(new Rect(screenPos.x, screenPos.y, 200, 100), star.GetName(), planetStyle);
	            }
	        }
		}
    }
}
