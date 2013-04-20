using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIManager : MonoBehaviour {

	public Texture ControlPanelBackground;
	
	float _DistanceFilter = 100f;
	float _ToggleControlPanelHeigth;
	bool _ToggleControlPanelUp;
	float _HeaderAlpha;
	bool _HeaderUp = true;
	string _FocusedStarName;
	
	void Start()
	{
	}
		
	void Update ()
	{
		if( Input.GetKeyDown(KeyCode.Tab) )
			_ToggleControlPanelUp = !_ToggleControlPanelUp;
		
		_HeaderUp = _ToggleControlPanelUp;
	}
	
	void OnGUI()
	{
		//Controller Background
		GUI.color = Color.grey;
		GUI.DrawTexture(new Rect(Screen.width - 130,0 - _ToggleControlPanelHeigth, 130, 200), ControlPanelBackground);
		GUI.color = Color.white;
		
		//Universe Scale
		Scaler.Scale = GUI.HorizontalSlider(new Rect(-15 + Screen.width - 100, 25 - _ToggleControlPanelHeigth, 100, 25), Scaler.Scale, 0, 10);
		GUI.Label(new Rect(-15 + Screen.width - 100, 5 - _ToggleControlPanelHeigth, 100, 25), "Scale Universe");
		
		//Distance Filter
		_DistanceFilter = GUI.HorizontalSlider(new Rect(-15 + Screen.width - 100, 65 - _ToggleControlPanelHeigth, 100, 25), _DistanceFilter, 0, 1000);
		GUI.Label(new Rect(-15 + Screen.width - 100, 45 - _ToggleControlPanelHeigth, 100, 25), "Distance Filter");
		
		//Toggle GUI
		if( GUI.Button(new Rect(-15 + Screen.width - 100, 165 - _ToggleControlPanelHeigth, 100, 25), "Toggle (Tab)" ) )
		{
			_ToggleControlPanelUp = !_ToggleControlPanelUp;
		}
		if( _ToggleControlPanelUp )
		{
			if( _ToggleControlPanelHeigth < 160 )
				_ToggleControlPanelHeigth += Time.deltaTime * 300f;
			else
				_ToggleControlPanelHeigth = 160;
		}
		else
		{
			if( _ToggleControlPanelHeigth > 0 )
				_ToggleControlPanelHeigth -= Time.deltaTime * 300f;
			else
				_ToggleControlPanelHeigth = 0;
		}
		
		//Hide / Show Star Header
		GUIStyle headerStyle = new GUIStyle();
		headerStyle.fontSize = 50;
		headerStyle.normal.textColor = new Color(255,255,255,_HeaderAlpha);
		GUI.Label(new Rect(Screen.width / 2 - headerStyle.CalcSize(new GUIContent(_FocusedStarName)).x / 2, 30, 100, 25), _FocusedStarName , headerStyle);
		if( _HeaderUp )
		{
			if( _HeaderAlpha < 1 )
				_HeaderAlpha += Time.deltaTime * 0.8f;
			else
				_HeaderAlpha = 1;
		}
		else
		{
			if( _HeaderAlpha > 0 )
				_HeaderAlpha -= Time.deltaTime * 0.8f;
			else
				_HeaderAlpha = 0;
		}
		
		//Planet,Star,Astroid Information
		GUIStyle informationStyle = new GUIStyle();
		informationStyle.fontSize = 40;
		informationStyle.normal.textColor = new Color(255,255,255,_HeaderAlpha);
		GUI.Label(new Rect(15, Screen.height * 0.3f, 100, 25), "Magnitude: 5134", informationStyle);
		GUI.Label(new Rect(15, Screen.height * 0.3f + 40, 100, 25), "Magnitude: 5134", informationStyle);
		GUI.Label(new Rect(15, Screen.height * 0.3f + 80, 100, 25), "Magnitude: 5134", informationStyle);
		GUI.Label(new Rect(15, Screen.height * 0.3f + 120, 100, 25), "Magnitude: 5134", informationStyle);
	}
	
	public void SetStarFocus(string starInFocus)
	{
		if( starInFocus == string.Empty || starInFocus == "" )
			_HeaderUp = false;
		else
		{
			_HeaderUp = true;
			_FocusedStarName = starInFocus;
		}
	}
	
}
