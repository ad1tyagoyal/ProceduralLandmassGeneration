using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainGenerationScript : MonoBehaviour
{
    [Range(10, 256)]
    public int resolution = 10;
    public SimplexNoiseFilterSettings noiseSettings;

    public Region[] mapRegions;

    private MeshFilter m_MeshFilter;
    private TerrainFace m_TerrainFace;


    private SimplexNoiseFilter noiseFilter;
    void OnValidate()
    {
        InitializeFaceMeshes();
        GenerateFaceMeshes();
        GenerateTerrainColorTexture(m_MeshFilter.sharedMesh.vertices);
    }

    private void InitializeFaceMeshes()
    {
        noiseFilter = new SimplexNoiseFilter(noiseSettings);
        if (m_MeshFilter == null) {
            m_MeshFilter = new MeshFilter();
            GameObject _faceMesh = new GameObject("Mesh GFx");
            _faceMesh.transform.parent = transform;
            _faceMesh.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            m_MeshFilter = _faceMesh.AddComponent<MeshFilter>();
            m_MeshFilter.sharedMesh = new Mesh();

        }
        m_TerrainFace = new TerrainFace(noiseFilter, resolution, m_MeshFilter.sharedMesh, Vector3.up);
    }

    private void GenerateTerrainColorTexture(Vector3[] vertices) {
        Texture2D _terrainTexture = new Texture2D(resolution, resolution);
        Color[] _color = new Color[resolution * resolution];

        for (int x = 0; x < resolution; x++) {
            for (int y = 0; y < resolution; y++) {
                float _heightValue = vertices[x * resolution + y].y;
                for (int i = 0; i < mapRegions.Length; i++) {
                    if(mapRegions[i].limit > _heightValue) {
                        _color[x * resolution + y] = mapRegions[i].color;
                        break;
                    }
                }

            }
            
        }

        _terrainTexture.SetPixels(_color);
        _terrainTexture.Apply();
        transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = _terrainTexture;
    }

    private void GenerateFaceMeshes()
    {
        m_TerrainFace.ConstructMesh();
        transform.GetChild(0).localScale = new Vector3(256, 1.0f, 256);
    }

    [System.Serializable]
    public class Region {
        public string name;
        public float limit;
        public Color color;
    }
}
