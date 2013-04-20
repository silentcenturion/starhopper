using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetsWithStars : MonoBehaviour
{
	struct NameData
	{
		public Star Star;
		public Vector2 ScreenPos;
		public float Distance;
	}
	
	public GUIManager guiManager;
	
	private List<NameData> _DataList;

    // Use this for initialization
    void Start()
    {
        LoadExoplanet.Load(StarPicker.Stars);
		_DataList = new List<NameData>();
    }
	
	void Update()
	{
		if( guiManager.ShowPlanets() )
		{
			_DataList.Clear();
			
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
					
					NameData newData = new NameData();
					newData.ScreenPos = screenPos;
					newData.Star = star;
					newData.Distance = distance;
					_DataList.Add(newData);
	            }
	        }
		}
	}

    void OnGUI()
    {
		GUIStyle planetStyle = new GUIStyle();
		foreach(NameData data in _DataList)
		{
			planetStyle.normal.textColor = new Color(255,255,255, 1 - (data.Distance / guiManager.GetPlanetFilter()));
					
			if( data.Star.Planets.Count > 0 )
        		GUI.Label(new Rect(data.ScreenPos.x, data.ScreenPos.y, 200, 100), data.Star.GetName()+"(Planets: " + data.Star.Planets.Count+")", planetStyle);
			else
        		GUI.Label(new Rect(data.ScreenPos.x, data.ScreenPos.y, 200, 100), data.Star.GetName(), planetStyle);
					
		}
    }
}
