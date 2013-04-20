using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class StarMesh : MonoBehaviour
{
    public Material Material;
#if UNITY_EDITOR
    [MenuItem("Ryyyyymd/Create Stars")]
    static void CreateStars()
    {
        Object[] starMeshes = Object.FindObjectsOfType(typeof(StarMesh));
        foreach (var starMesh in starMeshes)
        {
            DestroyImmediate((starMesh as StarMesh).gameObject);
        }

        Star[] stars = LoadStars.Load();
        int maxStarsPerMesh = 15000;
        for (int i = 0; i < stars.Length; i+=maxStarsPerMesh)
        {
            GenerateMesh(stars, i, Mathf.Min(maxStarsPerMesh, stars.Length - i));
        }
    }
#endif

    static void GenerateMesh(Star[] stars, int offset, int count)
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

        int vertIndex = 0;
        int uvIndex = 0;
        int triangleIndex = 0;
        for (int i = 0; i < starVectors.Length; i++)
        {
            int vert1 = vertIndex++;
            int vert2 = vertIndex++;
            int vert3 = vertIndex++;
            int vert4 = vertIndex++;

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
        mesh.name = "starMesh";
        mesh.RecalculateBounds();

        GameObject go = new GameObject("starMesh");
        StarMesh starMesh = go.AddComponent<StarMesh>();
        MeshFilter meshFilter = go.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterial = starMesh.Material;
    }
}
