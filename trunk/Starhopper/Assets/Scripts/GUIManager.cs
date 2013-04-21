using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIManager : MonoBehaviour
{
    public Texture ControlPanelBackground;
    public OrbitCamera OrbitCamera;

    float _NameFilter = 50f;
    float _ToggleControlPanelHeigth;
    bool _ToggleControlPanelUp;
    float _HeaderAlpha;
    bool _HeaderUp = false;
    string _FocusedStarName;
    string _Distance;
    string _Magnitude;
    string _Spectrum;
    string _ColorIndex;
    bool _ShowNameplates;
    bool _ShowOnlyWithPlanets = true;
    string _InputText;

    public bool IsMouseOverGui;

    public float Speed;

    void Start()
    {
        _InputText = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            _ToggleControlPanelUp = !_ToggleControlPanelUp;


    }

    void OnGUI()
    {
        if (Event.current.keyCode == KeyCode.Tab || Event.current.character == '\t')
            Event.current.Use();
        if (Event.current.keyCode == KeyCode.Return || Event.current.character == '\n')
        {
            foreach (Star star in StarPicker.Stars)
            {
                if (_InputText.ToLower() == star.GetName().ToLower())
                {
                    OrbitCamera.SetMode(CameraMode.Free);
                    OrbitCamera.OrbitLocation(star);
                    SetStarFocus(star);
                    GUI.SetNextControlName("");
                    GUI.FocusControl("");
                    break;
                }
            }
        }

        //Controller Background
        GUI.color = Color.grey;
        Rect rect = new Rect(Screen.width - 160, 0 - _ToggleControlPanelHeigth, 160, Screen.height);
        GUI.DrawTexture(rect, ControlPanelBackground);
        GUI.color = Color.white;

        Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        IsMouseOverGui = rect.Contains(mousePos);

        //Universe Scale
        Scaler.Scale = GUI.HorizontalSlider(new Rect(-15 + Screen.width - 130, 25 - _ToggleControlPanelHeigth, 130, 25), Scaler.Scale, 0.5f, 150);
        GUI.Label(new Rect(-15 + Screen.width - 130, 5 - _ToggleControlPanelHeigth, 130, 25), "Scale Universe");

        //Saturation
        Universe.Saturation = GUI.HorizontalSlider(new Rect(-15 + Screen.width - 130, 55 - _ToggleControlPanelHeigth, 130, 25), Universe.Saturation, 0.0f, 4f);
        GUI.Label(new Rect(-15 + Screen.width - 130, 35 - _ToggleControlPanelHeigth, 130, 25), "Saturation");

        //Checkbox
        _ShowNameplates = GUI.Toggle(new Rect(-15 + Screen.width - 130, 170 - _ToggleControlPanelHeigth, 130, 20), _ShowNameplates, " Show Names");
        _ShowOnlyWithPlanets = GUI.Toggle(new Rect(-15 + Screen.width - 130, 190 - _ToggleControlPanelHeigth, 130, 20), _ShowOnlyWithPlanets, " Only Planets");

        //Name Filter
        _NameFilter = GUI.HorizontalSlider(new Rect(-15 + Screen.width - 130, 230 - _ToggleControlPanelHeigth, 130, 25), _NameFilter, 50, 500);
        GUI.Label(new Rect(-15 + Screen.width - 130, 210 - _ToggleControlPanelHeigth, 130, 25), "Name Filter");

        //Text Field
        GUI.Label(new Rect(-15 + Screen.width - 130, 290 - _ToggleControlPanelHeigth, 130, 25), "Search (Enter)");
        _InputText = GUI.TextField(new Rect(-15 + Screen.width - 130, 310 - _ToggleControlPanelHeigth, 130, 25), _InputText, 30);

        //Buttons
        if (GUI.Button(new Rect(-15 + Screen.width - 130, 370 - _ToggleControlPanelHeigth, 130, 25), "Galaxy View"))
        {
            OrbitCamera.SetMode(CameraMode.Galaxy);
        }
        if (GUI.Button(new Rect(-15 + Screen.width - 130, 400 - _ToggleControlPanelHeigth, 130, 25), "Solar View"))
        {
            Sun.RemoveSolarSystems();
            OrbitCamera.SetMode(CameraMode.Solar);
        }
        if (GUI.Button(new Rect(-15 + Screen.width - 130, 430 - _ToggleControlPanelHeigth, 130, 25), "Free View"))
        {
            OrbitCamera.SetMode(CameraMode.Free);
        }
        if (OrbitCamera.CurrentCameraMode == CameraMode.Solar)
        {
            if (GUI.Button(new Rect(-15 + Screen.width - 130, 500 - _ToggleControlPanelHeigth, 130, 25), "Step Astroid"))
            {
                //Set AStroiud
            }
            if (GUI.Button(new Rect(-15 + Screen.width - 130, 530 - _ToggleControlPanelHeigth, 130, 25), "Top Diameter"))
            {
                AsteroidVoyeur.Instance.TopDiameter();
            }
            if (GUI.Button(new Rect(-15 + Screen.width - 130, 560 - _ToggleControlPanelHeigth, 130, 25), "Top Period"))
            {
                AsteroidVoyeur.Instance.TopPeriod();
            }
            if (GUI.Button(new Rect(-15 + Screen.width - 130, 590 - _ToggleControlPanelHeigth, 130, 25), "Top Eccentricity"))
            {
                AsteroidVoyeur.Instance.TopEccentricity();
            }
           
        }

        //GUI.Label(new Rect(1000, 800, 100, 100), "Speed: " + Speed.ToString("N2"));

        //Toggle GUI
        if (GUI.Button(new Rect(-15 + Screen.width - 130, Screen.height - 30 - _ToggleControlPanelHeigth, 130, 25), "Toggle (Tab)"))
        {
            _ToggleControlPanelUp = !_ToggleControlPanelUp;
        }
        if (_ToggleControlPanelUp)
        {
            if (_ToggleControlPanelHeigth < Screen.height - 30)
                _ToggleControlPanelHeigth += Time.deltaTime * 1500;
            else
                _ToggleControlPanelHeigth = Screen.height - 30;
        }
        else
        {
            if (_ToggleControlPanelHeigth > 0)
                _ToggleControlPanelHeigth -= Time.deltaTime * 1500f;
            else
                _ToggleControlPanelHeigth = 0;
        }

        //Hide / Show Star Header
        GUIStyle headerStyle = new GUIStyle();
        headerStyle.fontSize = 50;
        headerStyle.normal.textColor = new Color(255, 255, 255, _HeaderAlpha);
        GUI.Label(new Rect(Screen.width / 2 - headerStyle.CalcSize(new GUIContent(_FocusedStarName)).x / 2, 30, 100, 25), _FocusedStarName, headerStyle);
        if (_HeaderUp)
        {
            if (_HeaderAlpha < 1)
                _HeaderAlpha += Time.deltaTime * 0.8f;
            else
                _HeaderAlpha = 1;
        }
        else
        {
            if (_HeaderAlpha > 0)
                _HeaderAlpha -= Time.deltaTime * 0.8f;
            else
                _HeaderAlpha = 0;
        }

        //Planet,Star,Astroid Information
        GUIStyle informationStyle = new GUIStyle();
        informationStyle.fontSize = 20;
        informationStyle.normal.textColor = new Color(255, 255, 255, _HeaderAlpha);
        GUI.Label(new Rect(15, Screen.height * 0.3f, 0, 25), "Distance:", informationStyle);
        GUI.Label(new Rect(15, Screen.height * 0.3f + 30, 100, 25), "Magnitude:", informationStyle);
        GUI.Label(new Rect(15, Screen.height * 0.3f + 60, 100, 25), "Spectrum:", informationStyle);
        GUI.Label(new Rect(15, Screen.height * 0.3f + 90, 100, 25), "Color Index:", informationStyle);

        informationStyle.normal.textColor = new Color(255, 0, 255, _HeaderAlpha);
        GUI.Label(new Rect(150, Screen.height * 0.3f, 0, 25), _Distance, informationStyle);
        GUI.Label(new Rect(150, Screen.height * 0.3f + 30, 100, 25), _Magnitude, informationStyle);
        GUI.Label(new Rect(150, Screen.height * 0.3f + 60, 100, 25), _Spectrum, informationStyle);
        GUI.Label(new Rect(150, Screen.height * 0.3f + 90, 100, 25), _ColorIndex, informationStyle);

    }

    public void HideStarFocus()
    {
        _HeaderUp = false;
    }

    public bool ShowPlanets()
    {
        return _ShowNameplates;
    }

    public bool ShowOnlyWithPlanets()
    {
        return _ShowOnlyWithPlanets;
    }

    public float GetPlanetFilter()
    {
        return _NameFilter;
    }

    public void SetStarFocus(Star star)
    {
        _HeaderUp = true;
        _FocusedStarName = star.GetName();
        _Distance = star.Distance.ToString();
        _Magnitude = star.Mag.ToString();
        _Spectrum = star.Spectrum;
        _ColorIndex = star.ColorIndex.ToString();
    }
}
