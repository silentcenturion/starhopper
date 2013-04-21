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
    struct PlanetNameData
    {
        public Exoplanet Planet;
        public Vector2 ScreenPos;
        public float Distance;
    }

    public static Star PickedStar;

    public GUIManager guiManager;

    private List<NameData> _DataList;
    private List<PlanetNameData> _PlanetDataList;

    GUIStyle planetStyle;

    // Use this for initialization
    void Start()
    {
        LoadExoplanet.Load(StarPicker.Stars);
        _DataList = new List<NameData>();
        _PlanetDataList = new List<PlanetNameData>();
        planetStyle = new GUIStyle();
    }

    void LateUpdate()
    {
        Vector3 camPos = Camera.mainCamera.transform.position;

        _DataList.Clear();
        if (guiManager.ShowPlanets())
        {
            foreach (Star star in StarPicker.Stars)
            {
                List<Exoplanet> planets = star.Planets;

                if (guiManager.ShowOnlyWithPlanets() && star.Planets.Count == 0)
                    continue;

                Vector3 starPos = new Vector3(star.X, star.Y, star.Z) * Scaler.Scale;
                float distance3D = Vector3.SqrMagnitude(camPos - starPos);

                float planetFilter = guiManager.GetPlanetFilter();
                if (guiManager.ShowOnlyWithPlanets())
                    planetFilter *= 10;

                if (distance3D < planetFilter * planetFilter * Scaler.Scale)
                {
                    Bounds bounds = new Bounds(starPos, Vector3.one);
                    if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), bounds) == false)
                        continue;

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

        _PlanetDataList.Clear();
        if (PickedStar != null && PickedStar.Planets.Count > 0)
        {
            for (int i = 0; i < Sun.Instance._Planets.Length; i++ )
            {
                Transform planetTransform = Sun.Instance._Planets[i];
                Exoplanet planet = PickedStar.Planets[i];

                Vector3 planetPos = planetTransform.position;
                float distance3D = Vector3.SqrMagnitude(camPos - planetPos);

                //if (distance3D < guiManager.GetPlanetFilter() * guiManager.GetPlanetFilter() * Scaler.Scale)
                {
                    Bounds bounds = new Bounds(planetPos, Vector3.one);
                    if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), bounds) == false)
                        continue;

                    Vector3 screenPos3D = Camera.mainCamera.WorldToScreenPoint(planetPos);
                    screenPos3D.y = Screen.height - screenPos3D.y;

                    if (screenPos3D.z < 0)
                        continue;

                    float distance = Vector3.Distance(planetPos, camPos);
                    Vector2 screenPos = new Vector2(screenPos3D.x, screenPos3D.y);

                    PlanetNameData newData = new PlanetNameData();
                    newData.ScreenPos = screenPos;
                    newData.Planet = planet;
                    newData.Distance = distance;
                    _PlanetDataList.Add(newData);
                }
            }
        }

    }

    void OnGUI()
    {
        float planetFilter = guiManager.GetPlanetFilter();
        if (guiManager.ShowOnlyWithPlanets())
            planetFilter *= 10;

        foreach (NameData data in _DataList)
        {
            planetStyle.normal.textColor = new Color(255, 255, 255, 1 - (data.Distance / planetFilter));

            if (data.Star.Planets.Count > 0)
                GUI.Label(new Rect(data.ScreenPos.x, data.ScreenPos.y, 200, 100), data.Star.GetName() + " (Planets: " + data.Star.Planets.Count + ")", planetStyle);
            else
                GUI.Label(new Rect(data.ScreenPos.x, data.ScreenPos.y, 200, 100), data.Star.GetName(), planetStyle);
        }

        foreach (PlanetNameData data in _PlanetDataList)
        {
            planetStyle.normal.textColor = new Color(255, 255, 255, 1 - (data.Distance / planetFilter));

            GUI.Label(new Rect(data.ScreenPos.x, data.ScreenPos.y, 200, 100), data.Planet.PlanetName, planetStyle);
        }
    }
}
