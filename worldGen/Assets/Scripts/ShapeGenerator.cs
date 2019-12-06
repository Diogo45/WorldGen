using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    public MinMax elevMinMax;

    public void UpdateSettings(ShapeSettings settings)
    {
        elevMinMax = new MinMax();
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerVal = 0f;
        float elevation = 0f;
        if(noiseFilters.Length > 0)
        {
            firstLayerVal = noiseFilters[0].Eval(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled) elevation = firstLayerVal;
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (!settings.noiseLayers[i].enabled) continue;
            float mask = (settings.noiseLayers[i].useFirstLayerAsMask ? firstLayerVal : 1);
            elevation += noiseFilters[i].Eval(pointOnUnitSphere) * mask;
        }

        elevation = settings.planetRadius * (1 + elevation);
        elevMinMax.AddValue(elevation);

        return pointOnUnitSphere * elevation;
    }

}
