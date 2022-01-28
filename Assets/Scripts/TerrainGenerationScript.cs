using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainGenerationScript : MonoBehaviour
{
    [Range(10, 256)]
    public int resolution = 10;

    public int width = 10;
    public int height = 10;

    public SimplexNoiseFilterSettings noiseSettings;

    private MeshFilter[] m_MeshFilters;
    private TerrainFace[] m_TerrainFaces;


    private SimplexNoiseFilter noiseFilter;
    void OnValidate()
    {
        InitializeFaceMeshes();
        GenerateFaceMeshes();
    }

    private void InitializeFaceMeshes()
    {
        noiseFilter = new SimplexNoiseFilter(noiseSettings);
        if (m_MeshFilters == null || m_MeshFilters.Length == 0)
            m_MeshFilters = new MeshFilter[1];

        m_TerrainFaces = new TerrainFace[1];

        Vector3[] _directions = { Vector3.up,
                                     Vector3.down,
                                     Vector3.right,
                                     Vector3.left,
                                     Vector3.back,
                                     Vector3.forward };

        for (int i = 0; i < 1; i++)
        {
            if (m_MeshFilters[i] == null)
            {
                GameObject _faceMesh = new GameObject("Face Mesh" + i);
                _faceMesh.transform.parent = transform;
                _faceMesh.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                m_MeshFilters[i] = _faceMesh.AddComponent<MeshFilter>();
                m_MeshFilters[i].sharedMesh = new Mesh();
            }
            m_TerrainFaces[i] = new TerrainFace(noiseFilter, resolution, m_MeshFilters[i].sharedMesh, _directions[i]);
        }
    }

    private void GenerateFaceMeshes()
    {
        for (int i = 0; i < 1; i++)
        {
            m_TerrainFaces[i].ConstructMesh();
            this.transform.localScale = new Vector3(width, 1.0f, height);
        }
    }
}
