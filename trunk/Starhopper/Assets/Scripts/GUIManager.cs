using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIManager : MonoBehaviour {

	public Texture ControlPanelBackground;
	
	float _DistanceFilter = 100f;
	float _ToggleControlPanelHeigth;
	bool _ToggleControlPanelUp;
	float _HeaderHeigth;
	bool _HeaderUp = true;
	string _FocusedStarName;
	
	void Start()
	{
	}
		
	void Update ()
	{
		if( Input.GetKeyDown(KeyCode.Tab) )
			_ToggleControlPanelUp = !_ToggleControlPanelUp;
	}
	
	void OnGUI()
	{
		//Controller Background
		GUI.color = Color.grey;
		GUI.DrawTexture(new Rect(0,0 - _ToggleControlPanelHeigth, 130, 200), ControlPanelBackground);
		GUI.color = Color.white;
		
		//Universe Scale
		Scaler.Scale = GUI.HorizontalSlider(new Rect(15, 25 - _ToggleControlPanelHeigth, 100, 25), Scaler.Scale, 0, 10);
		GUI.Label(new Rect(15, 5 - _ToggleControlPanelHeigth, 100, 25), "Scale Universe");
		
		//Distance Filter
		_DistanceFilter = GUI.HorizontalSlider(new Rect(15, 65 - _ToggleControlPanelHeigth, 100, 25), _DistanceFilter, 0, 1000);
		GUI.Label(new Rect(15, 45 - _ToggleControlPanelHeigth, 100, 25), "Distance Filter");
		
		//Hide / Show Header
		GUIStyle headerStyle = new GUIStyle();
		headerStyle.fontSize = 50;
		headerStyle.normal.textColor = Color.white;
		GUI.Label(new Rect(Screen.width / 2 - headerStyle.CalcSize(new GUIContent(_FocusedStarName)).x / 2, 30 - _HeaderHeigth, 100, 25), _FocusedStarName , headerStyle);
		if( _HeaderUp )
		{
			if( _HeaderHeigth < 160 )
				_HeaderHeigth += Time.deltaTime * 300f;
			else
				_HeaderHeigth = 160;
		}
		else
		{
			if( _HeaderHeigth > 0 )
				_HeaderHeigth -= Time.deltaTime * 300f;
			else
				_HeaderHeigth = 0;
		}
		
		//Toggle GUI
		if( GUI.Button(new Rect(15, 165 - _ToggleControlPanelHeigth, 100, 25), "Toggle (Tab)" ) )
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
	}
	
	public void SetStarFocus(string starInFocus)
	{
		if( starInFocus == string.Empty || starInFocus == "" )
			_HeaderUp = true;
		else
		{
			_HeaderUp = false;
			_FocusedStarName = starInFocus;
		}
	}
	
}
