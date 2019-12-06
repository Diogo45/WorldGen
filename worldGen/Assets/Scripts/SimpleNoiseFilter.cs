using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.SimpleNoiseSettings settings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings noiseSettings)
    {
        this.settings = noiseSettings;
    }

    public float Eval(Vector3 point)
    {
        float noiseVal = 0f;
        float freq = settings.baseRoughness;
        float amp = 1f;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = noise.Evaluate(point * freq + settings.centre);
            noiseVal += (v + 1) * .5f * amp;
            amp *= settings.persistence;
            freq *= settings.roughness;
        }

        noiseVal = Mathf.Max(0, noiseVal - settings.minValue);
        return noiseVal * settings.strength;
    }
}
