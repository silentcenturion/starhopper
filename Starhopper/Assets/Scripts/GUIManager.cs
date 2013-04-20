using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIManager : MonoBehaviour
{
    public Texture ControlPanelBackground;

    float _DistanceFilter = 100f;
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

    public bool IsMouseOverGui;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            _ToggleControlPanelUp = !_ToggleControlPanelUp;
    }

    void OnGUI()
    {
        //Controller Background
        GUI.color = Color.grey;
        Rect rect = new Rect(Screen.width - 130, 0 - _ToggleControlPanelHeigth, 130, 200);
        GUI.DrawTexture(rect, ControlPanelBackground);
        GUI.color = Color.white;

        Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        IsMouseOverGui = rect.Contains(mousePos);

        //Universe Scale
        Scaler.Scale = GUI.HorizontalSlider(new Rect(-15 + Screen.width - 100, 25 - _ToggleControlPanelHeigth, 100, 25), Scaler.Scale, 0.5f, 150);
        GUI.Label(new Rect(-15 + Screen.width - 100, 5 - _ToggleControlPanelHeigth, 100, 25), "Scale Universe");

        //Distance Filter
        _DistanceFilter = GUI.HorizontalSlider(new Rect(-15 + Screen.width - 100, 65 - _ToggleControlPanelHeigth, 100, 25), _DistanceFilter, 0, 1000);
        GUI.Label(new Rect(-15 + Screen.width - 100, 45 - _ToggleControlPanelHeigth, 100, 25), "Distance Filter");
		
		//Checkbox
		_ShowNameplates = GUI.Toggle(new Rect(-15 + Screen.width - 100, 90	 - _ToggleControlPanelHeigth, 130, 20), _ShowNameplates, " Show Names"); 
		
		//Planet Filter
        _NameFilter = GUI.HorizontalSlider(new Rect(-15 + Screen.width - 100, 135 - _ToggleControlPanelHeigth, 100, 25), _NameFilter, 50, 500);
        GUI.Label(new Rect(-15 + Screen.width - 100, 115 - _ToggleControlPanelHeigth, 100, 25), "Name Filter");
		
        //Toggle GUI
        if (GUI.Button(new Rect(-15 + Screen.width - 100, 165 - _ToggleControlPanelHeigth, 100, 25), "Toggle (Tab)"))
        {
            _ToggleControlPanelUp = !_ToggleControlPanelUp;
        }
        if (_ToggleControlPanelUp)
        {
            if (_ToggleControlPanelHeigth < 160)
                _ToggleControlPanelHeigth += Time.deltaTime * 300f;
            else
                _ToggleControlPanelHeigth = 160;
        }
        else
        {
            if (_ToggleControlPanelHeigth > 0)
                _ToggleControlPanelHeigth -= Time.deltaTime * 300f;
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
        GUI.Label(new Rect(150, Screen.height * 0.3f + 30, 100, 25),  _Magnitude, informationStyle);
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
