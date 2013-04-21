using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Universe : MonoBehaviour
{
    public Material SunMaterial;
    public Material PlanetMaterial;
    public Material StarMaterial;
	public Material AsteroidMaterial;

    public static float Saturation = 1;

    void Update()
    {
        Shader.SetGlobalFloat("_Saturation", Saturation);
    }
}
