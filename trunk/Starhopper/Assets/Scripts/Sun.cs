using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;


public class Planet
{
    public float Distance;
    public float Size;

    public Planet(float distance, float size)
    {
        Distance = distance;
        Size = size;
    }

}

public class Sun : MonoBehaviour
{
    float _Rotation;
    public float RotationSpeed = 3;
    public float Scale = 1;

    private Vector3 OriginalPos;

    Transform[] _Planets = new Transform[0];

    // Use this for initialization
    void Start()
    {
        _Rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _Rotation += Time.deltaTime;
        Quaternion rot = Quaternion.Euler(_Rotation * RotationSpeed, 0f, 0f);
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rot, new Vector3(1, 1, 1));
        GetComponent<MeshRenderer>().sharedMaterial.SetMatrix("_Rotation", m);

        transform.localScale = Vector3.one * Scale;

        // Updat1e Orbit
        if (_Planets != null && _Planets.Length > 0)
        {
            foreach (Transform planet in _Planets)
            {
                Vector3 euler = planet.transform.parent.rotation.eulerAngles;
                planet.transform.parent.rotation = Quaternion.Euler(0, euler.y + Time.deltaTime * 50, 0);
            }
        }

        transform.position = OriginalPos * Scaler.Scale;
    }



#if UNITY_EDITOR
    [MenuItem("Space App Challenge/Create SolarSystem")]
    public static void CreateSolarSystem()
    {
        CreateSolarSystem(null);
    }
#endif

    public static void CreateSolarSystem(Star star)
    {
        Object[] solarSystems = Object.FindObjectsOfType(typeof(Sun));
        foreach (var solarSystem in solarSystems)
        {
            DestroyImmediate((solarSystem as Sun).gameObject);
        }


        Planet[] planets = new Planet[]
        {
            new Planet(1f, 0.2f),            
            new Planet(3f, 0.1f),            
            new Planet(6f, 0.4f)
        };

        Sun sun = GenerateSun(star);
    }
    
    public void CreatePlanets(System.Collections.Generic.List<Exoplanet> planets)
    {
        Universe universe = Object.FindObjectOfType(typeof(Universe)) as Universe;

        _Planets = new Transform[planets.Count];
        for (int i = 0; i < planets.Count; i++)
        {
            Exoplanet planet = planets[i];
            GameObject planetParent = new GameObject("planetParent");
            GameObject go = new GameObject("planet");
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            Mesh mesh = GenerateMesh(Color.white);
            meshFilter.sharedMesh = mesh;
            meshRenderer.sharedMaterial = universe.PlanetMaterial;


            planetParent.transform.parent = transform;
            planetParent.transform.localPosition = Vector3.zero;
            go.transform.parent = planetParent.transform;

            planetParent.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            go.transform.localPosition = new Vector3(planet.PlanetaryRadius, 0, 0);
            go.transform.localScale = Vector3.one * 1;

            _Planets[i] = go.transform;
        }
    }

    static Sun GenerateSun(Star star)
    {
        GameObject go = new GameObject("sun");
        Sun sun = go.AddComponent<Sun>();
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();


        Universe universe = Object.FindObjectOfType(typeof(Universe)) as Universe;
        meshRenderer.sharedMaterial = universe.SunMaterial;

        Mesh mesh = GenerateMesh(Color.white);
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;

        sun.OriginalPos = new Vector3(star.X, star.Y, star.Z);
        
        if (star.Planets != null && star.Planets.Count > 0)
        {
            sun.CreatePlanets(star.Planets);
        }

        return sun;
    }
    
    static Mesh GenerateMesh(Color color)
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];
        Color[] colors = new Color[4];

        int vertIndex = 0;
        int uvIndex = 0;
        int triangleIndex = 0;

        int vert1 = vertIndex++;
        int vert2 = vertIndex++;
        int vert3 = vertIndex++;
        int vert4 = vertIndex++;

        //// Abs MAg
        //float absMag = stars[i + offset].AbsMag;
        //float normalizedMag = (absMag - minMag) / (maxMag - minMag);
        //color.a = 1 - normalizedMag;

        colors[vert1] = color;
        colors[vert2] = color;
        colors[vert3] = color;
        colors[vert4] = color;

        Vector3 position = Vector3.zero;
        vertices[vert1] = position;
        vertices[vert2] = position;
        vertices[vert3] = position;
        vertices[vert4] = position;

        uvs[uvIndex++] = new Vector2(1, 1);
        uvs[uvIndex++] = new Vector2(1, -1);
        uvs[uvIndex++] = new Vector2(-1, -1);
        uvs[uvIndex++] = new Vector2(-1, 1);

        triangles[triangleIndex++] = vert1;
        triangles[triangleIndex++] = vert2;
        triangles[triangleIndex++] = vert3;

        triangles[triangleIndex++] = vert3;
        triangles[triangleIndex++] = vert4;
        triangles[triangleIndex++] = vert1;


        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.colors = colors;
        mesh.name = "sunOrPlanet";
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        
        return mesh;
    }
}
