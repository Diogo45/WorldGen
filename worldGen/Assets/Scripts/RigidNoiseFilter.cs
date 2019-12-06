using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter: INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.RigidNoiseSettings settings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings noiseSettings)
    {
        this.settings = noiseSettings;
    }

    public float Eval(Vector3 point)
    {
        float noiseVal = 0f;
        float freq = settings.baseRoughness;
        float amp = 1f;
        float weigth = 1f;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = 1f - Mathf.Abs(noise.Evaluate(point * freq + settings.centre));
            v *= v;
            v *= weigth;
            weigth = Mathf.Clamp01(v * settings.weigthMult);
            noiseVal += v * amp;
            amp *= settings.persistence;
            freq *= settings.roughness;
        }

        noiseVal = Mathf.Max(0, noiseVal - settings.minValue);
        return noiseVal * settings.strength;
    }
}
