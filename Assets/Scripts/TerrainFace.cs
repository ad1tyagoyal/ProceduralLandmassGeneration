using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    SimplexNoiseFilter m_NoiseFilter;
    private int m_Resolution;
    private Mesh m_Mesh;
    private Vector3 m_LocalUp, m_AxisA, m_AxisB;

    public TerrainFace(SimplexNoiseFilter noiseFilter, int resolution, Mesh mesh, Vector3 localUp)
    {
        m_NoiseFilter = noiseFilter;
        m_Resolution = resolution;
        m_Mesh = mesh;
        m_LocalUp = localUp;
        m_AxisA = new Vector3(m_LocalUp.y, m_LocalUp.z, m_LocalUp.x);
        m_AxisB = Vector3.Cross(m_LocalUp, m_AxisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[(int)Mathf.Pow(m_Resolution, 2)];
        int[] triangleIndices = new int[(int)(Mathf.Pow((m_Resolution - 1), 2) * 6)];
        Vector2[] uvs = new Vector2[(int)Mathf.Pow(m_Resolution, 2)];

        {
            int verticesIndex = 0, triangleIndex = 0;

            //foreach vertices
            for (int y = 0; y < m_Resolution; y++)
            {
                for (int x = 0; x < m_Resolution; x++)
                {

                    Vector2 percentage = new Vector3(x, y) / (m_Resolution - 1);
                    Vector3 pointOnUnitCube = m_LocalUp + ((percentage.x - 0.5f) * 2 * m_AxisA)
                                                        + ((percentage.y - 0.5f) * 2 * m_AxisB);

                    pointOnUnitCube.y *= m_NoiseFilter.Evaluate(ref pointOnUnitCube);
                    vertices[verticesIndex] = pointOnUnitCube;
                    uvs[verticesIndex] = new Vector2(x / (float)m_Resolution, y / (float)m_Resolution);

                    if (x != (m_Resolution - 1) && y != (m_Resolution - 1))
                    {
                        triangleIndices[triangleIndex] = verticesIndex;
                        triangleIndices[triangleIndex + 1] = verticesIndex + 1;
                        triangleIndices[triangleIndex + 2] = verticesIndex + m_Resolution + 1;

                        triangleIndices[triangleIndex + 3] = verticesIndex;
                        triangleIndices[triangleIndex + 4] = verticesIndex + m_Resolution + 1;
                        triangleIndices[triangleIndex + 5] = verticesIndex + m_Resolution;

                        triangleIndex += 6;
                    }

                    verticesIndex++;
                }
            }
        }

        m_Mesh.Clear();
        m_Mesh.vertices = vertices;
        m_Mesh.triangles = triangleIndices;
        m_Mesh.uv = uvs;
        m_Mesh.RecalculateNormals();
    }
}
