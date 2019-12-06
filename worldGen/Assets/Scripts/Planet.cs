using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };




    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public FaceRenderMask faceRenderMask;

    public bool animateNoise = false;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    ShapeGenerator shapeGenerator =  new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    [HideInInspector]
    public bool settingsShapeFold; 
    [HideInInspector]
    public bool settingsColorFold;

    [HideInInspector, SerializeField]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    void Initialize()
    {
        colorGenerator.UpdateSettings(colorSettings);
        shapeGenerator.UpdateSettings(shapeSettings);
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];

        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;
                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMat;
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }


    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();

    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.active)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colorGenerator.UpdateElev(shapeGenerator.elevMinMax);
    }


    public void OnShapeSettingsUpdated()
    {
        if (!autoUpdate) return;
        Initialize();
        GenerateMesh();
    }


    public void OnColorSettingsUpdated()
    {
        if (!autoUpdate) return;
        Initialize();
        GenerateColors();
    }

    void GenerateColors()
    {

        colorGenerator.UpdateColors();
    }
}
