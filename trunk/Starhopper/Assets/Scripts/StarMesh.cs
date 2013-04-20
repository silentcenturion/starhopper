using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class StarMesh : MonoBehaviour
{
    public Material Material;
#if UNITY_EDITOR
    [MenuItem("Space App Challenge/Create Stars")]
    static void CreateStars()
    {
        Object[] starMeshes = Object.FindObjectsOfType(typeof(StarMesh));
        foreach (var starMesh in starMeshes)
        {
            DestroyImmediate((starMesh as StarMesh).gameObject);
        }

        Star[] stars = LoadStars.Load();

        float min = float.MaxValue;
        float max = float.MinValue;
        GetMinMaxAbsMag(stars, ref min, ref max);

        int maxStarsPerMesh = 15000;
        for (int i = 0; i < stars.Length; i += maxStarsPerMesh)
        {
            GenerateMesh(stars, i, Mathf.Min(maxStarsPerMesh, stars.Length - i), min, max);
        }
    }
#endif

    static Color getColor(int r, int g, int b)
    {
        return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, 1);
    }

    static Color[] colors = new Color[] 
    {
        getColor(157, 180, 255),
        getColor(170, 191, 255),
        getColor(202, 216, 255),
        getColor(251, 248, 255),
        getColor(255, 244, 232),
        getColor(255, 221, 180),
        getColor(255, 189, 111),
        getColor(248, 66, 53),
        getColor(186, 48, 89),
        getColor(96, 81, 112),
    };

    static Dictionary<string, int> _ClassificationToColorIndex = new Dictionary<string, int>()
    {
     { "o", 0 },
     { "b", 1 },
     { "a", 2 },
     { "f", 3 },
     { "g", 4 },
     { "k", 5 },
     { "m", 6 },
     { "l", 7 },
     { "t", 8 },
     { "y", 9 },   
    };

    static Color GetStarColor(char theChar, string spectrum)
    {
        int colorModifier = 0;
        if (spectrum.Length > 1 && char.IsDigit(spectrum[1]))
            int.TryParse(spectrum[1].ToString(), out colorModifier);
        int colorIndex;
        if (_ClassificationToColorIndex.TryGetValue(char.ToLowerInvariant(theChar).ToString(), out colorIndex))
        {
            Color color1 = colors[colorIndex];
            if (colorIndex == colors.Length - 1)
                return color1;
            Color color2 = colors[colorIndex + 1];
            return Color.Lerp(color1, color2, (float)colorModifier / 10);
        }
        return new Color(1, 0, 0, 1);
    }

    static void GetMinMaxAbsMag(Star[] star, ref float min, ref float max)
    {
        for (int i = 0; i < star.Length; i++)
        {
            if (min > star[i].AbsMag)
                min = star[i].AbsMag;
            if (max < star[i].AbsMag)
                max = star[i].AbsMag;
        }
    }

    static void GenerateMesh(Star[] stars, int offset, int count, float minMag, float maxMag)
    {
        Vector3[] starVectors = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            starVectors[i] = new Vector3(stars[i + offset].X, stars[i + offset].Y, stars[i + offset].Z);

            //starVectors[i] = Random.onUnitSphere * Random.Range(-10000, 10000);
        }

        Vector3[] vertices = new Vector3[starVectors.Length * 4];
        Vector2[] uvs = new Vector2[starVectors.Length * 4];
        int[] triangles = new int[starVectors.Length * 6];
        Color[] colors = new Color[starVectors.Length * 4];

        int vertIndex = 0;
        int uvIndex = 0;
        int triangleIndex = 0;
        for (int i = 0; i < starVectors.Length; i++)
        {
            int vert1 = vertIndex++;
            int vert2 = vertIndex++;
            int vert3 = vertIndex++;
            int vert4 = vertIndex++;



            Color color = Color.green;
            string spectrum = stars[i + offset].Spectrum;
            if (spectrum.Length > 0)
                color = GetStarColor(spectrum[0], spectrum);

            // Abs MAg
            float absMag = stars[i + offset].AbsMag;
            float normalizedMag = (absMag - minMag) / (maxMag - minMag);
            color.a = 1 - normalizedMag;

            colors[vert1] = color;
            colors[vert2] = color;
            colors[vert3] = color;
            colors[vert4] = color;


            vertices[vert1] = starVectors[i];
            vertices[vert2] = starVectors[i];
            vertices[vert3] = starVectors[i];
            vertices[vert4] = starVectors[i];

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
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.name = "starMesh";
        mesh.RecalculateBounds();

        GameObject go = new GameObject("starMesh");
        StarMesh starMesh = go.AddComponent<StarMesh>();
        MeshFilter meshFilter = go.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
        go.AddComponent<Scaler>();
        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterial = starMesh.Material;
    }
}
