using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class StarMesh : MonoBehaviour
{
    [ContextMenu("Generate Mesh")]
    void GenerateMesh()
    {        
        int starCount = 14000;

        Vector3[] stars = new Vector3[starCount];
        for (int i = 0; i < starCount; i++)
        {
            stars[i] = Random.onUnitSphere * Random.Range(-10000, 10000);
        }

        Vector3[] vertices = new Vector3[stars.Length * 4];
        Vector2[] uvs = new Vector2[stars.Length * 4];
        int[] triangles = new int[stars.Length * 6];

        int vertIndex = 0;
        int uvIndex = 0;
        int triangleIndex = 0;
        for (int i = 0; i < stars.Length; i++)
        {
            int vert1 = vertIndex++;
            int vert2 = vertIndex++;
            int vert3 = vertIndex++;
            int vert4 = vertIndex++;
            
            vertices[vert1] = stars[i];
            vertices[vert2] = stars[i];
            vertices[vert3] = stars[i];
            vertices[vert4] = stars[i];

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
        
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;
    }
}
