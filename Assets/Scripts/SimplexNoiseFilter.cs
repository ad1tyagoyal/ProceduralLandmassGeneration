using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimplexNoiseFilter
{
    SimplexPerlinNoise noise = new SimplexPerlinNoise();
    SimplexNoiseFilterSettings noiseSettings;

    public SimplexNoiseFilter(SimplexNoiseFilterSettings settings)
    {
        noiseSettings = settings;
    }

    public float Evaluate(ref Vector3 point)
    {
        float noiseValue = 0;
        float amplitude = 1;
        float frequency = noiseSettings.baseLacunarity;

        for (int i = 0; i < noiseSettings.noOfOctaves; i++)
        {
            float tempNoise = noise.Evaluate(point * frequency + noiseSettings.offset);
            noiseValue += (tempNoise + 1) * 0.5f * amplitude;
            amplitude *= noiseSettings.persistence;
            frequency *= noiseSettings.lacunarity;
        }
        noiseValue = Mathf.Max(0.0f, (noiseValue - noiseSettings.minValue));
        return noiseValue * noiseSettings.strength;
    }

}

[System.Serializable]
public class SimplexNoiseFilterSettings
{
    [Range(1, 8)]
    public int noOfOctaves = 1;
    public float strength = 1;
    public float baseLacunarity = 1;
    public float lacunarity = 1;
    public float persistence = 1;
    public Vector3 offset = new Vector3(1.0f, 1.0f, 1.0f);
    public float minValue = 1;
}