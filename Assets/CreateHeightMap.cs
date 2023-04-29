using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateHeightMap : MonoBehaviour
{
    [SerializeField] int totalRectangleX;
    [SerializeField] int totalRectangleZ;
    [SerializeField] float highScale;
    [SerializeField] Texture2D heightMap;

    int totalVerticesX;
    int totalVerticesZ;

    int totalVertices = 0;

    const int totalIndeciesPerRect = 6;
    int totalIndecies;

    Vector3[] vertices;
    int[] indecies;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    [SerializeField] Material material;

    private void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        totalVerticesX = totalRectangleX + 1;
        totalVerticesZ = totalRectangleZ + 1;
        totalVertices = totalVerticesX * totalVerticesZ;

        totalIndecies = totalRectangleX * totalRectangleZ * totalIndeciesPerRect;

        vertices = new Vector3[totalVertices];

        CreateIndeciesAndVertices();

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.triangles = indecies;
        meshRenderer.material = material;
    }

    void CreateIndeciesAndVertices()
    {
        for (int z = 0; z < totalVerticesZ; z++)
        {
            for (int x = 0; x < totalVerticesX; x++)
            {
                Color pixel = heightMap.GetPixel(-x, -z);
                int i = x + z * totalVerticesX;
                vertices[i] = new Vector3(-x, pixel.r * highScale, z);
            }
        }
        indecies = new int[totalIndecies];
        int currentIndex = 0;
        int currentRow = 1;

        for (int i = 0; i < totalIndecies; i += 6)
        {
            indecies[i + 0] = currentIndex;
            indecies[i + 1] = currentIndex + 1;
            indecies[i + 2] = currentIndex + 1 + totalVerticesX;

            indecies[i + 3] = currentIndex + 1 + totalVerticesX;
            indecies[i + 4] = currentIndex + totalVerticesX;
            indecies[i + 5] = currentIndex;

            currentIndex++;

            if (currentIndex >= totalVerticesX * currentRow - 1)
            {
                currentIndex++;
                currentRow++;
            }
        }
    }
}
