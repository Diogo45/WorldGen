﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor ColorEditor; 
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if(GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }


        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.settingsShapeFold, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.settingsColorFold, ref ColorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool fold, ref Editor editor)
    {
        if (settings)
        {
            fold = EditorGUILayout.InspectorTitlebar(fold, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                //Draws Horizontal Bar

                if (fold)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();
                    //Calls if values changed
                    if (check.changed)
                    {
                        onSettingsUpdated?.Invoke();
                    }
                }

            }
        }
    }

    private void OnEnable()
    {
        planet = (Planet) target;
    }
}
