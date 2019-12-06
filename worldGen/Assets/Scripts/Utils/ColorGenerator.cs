using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettings settings;
    Texture2D texture;
    const int textRes = 50;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if(!texture) 
            texture = new Texture2D(textRes, 1);
    }

    public void UpdateElev(MinMax minMax)
    {
        settings.planetMat.SetVector("_elevationMinMax", new Vector4(minMax.Min, minMax.Max, 0, 0));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[textRes];
        for (int i = 0; i < textRes; i++)
        {
            colors[i] = settings.gradient.Evaluate(i / (textRes - 1f));
        }
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMat.SetTexture("_texture", texture);

    }
}
