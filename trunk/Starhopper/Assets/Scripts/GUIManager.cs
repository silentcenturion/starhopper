using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIManager : MonoBehaviour {

	public Texture TestTexture;

	private float UniverseSliderValue = 1.0f;
	
	void Start()
	{
	}
		
	void UpdateHeader ()
	{
	}
	
	void OnGUI()
	{
		GUI.color = Color.grey;
		GUI.DrawTexture(new Rect(0,0, 130, 200), TestTexture);
		GUI.color = Color.white;
		UniverseSliderValue = GUI.HorizontalSlider(new Rect(15, 25, 100, 25), UniverseSliderValue, 0, 10);
		GUI.Label(new Rect(15, 5, 100, 25), "Scale Universe");
	}
}
