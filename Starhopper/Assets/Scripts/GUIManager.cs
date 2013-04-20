using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIManager : MonoBehaviour {

	public Texture ControlPanelBackground;
	
	float _DistanceFilter = 100f;
	float _ToggleHeigth;
	bool _ToggleUp;
	
	void Start()
	{
	}
		
	void Update ()
	{
		if( Input.GetKeyDown(KeyCode.Tab) )
			_ToggleUp = !_ToggleUp;
	}
	
	void OnGUI()
	{
		GUI.color = Color.grey;
		GUI.DrawTexture(new Rect(0,0 - _ToggleHeigth, 130, 200), ControlPanelBackground);
		GUI.color = Color.white;
		
		//Universe Scale
		Scaler.Scale = GUI.HorizontalSlider(new Rect(15, 25 - _ToggleHeigth, 100, 25), Scaler.Scale, 0, 10);
		GUI.Label(new Rect(15, 5 - _ToggleHeigth, 100, 25), "Scale Universe");
		
		//Distance Filter
		_DistanceFilter = GUI.HorizontalSlider(new Rect(15, 65 - _ToggleHeigth, 100, 25), _DistanceFilter, 0, 1000);
		GUI.Label(new Rect(15, 45 - _ToggleHeigth, 100, 25), "Distance Filter");
		
		//Toggle GUI
		if( GUI.Button(new Rect(15, 165 - _ToggleHeigth, 100, 25), "Toggle (Tab)" ) )
		{
			_ToggleUp = !_ToggleUp;
		}
		if( _ToggleUp )
		{
			if( _ToggleHeigth < 160 )
				_ToggleHeigth += Time.deltaTime * 300f;
			else
				_ToggleHeigth = 160;
		}
		else
		{
			if( _ToggleHeigth > 0 )
				_ToggleHeigth -= Time.deltaTime * 300f;
			else
				_ToggleHeigth = 0;
		}
	}
}
