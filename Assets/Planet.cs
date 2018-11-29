using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColourGenerator colourGenerator = new ColourGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    public float speed = 20f;

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
        //TODO: Generate the planet once when playmode begins
        //GeneratePlanet();
    }

    public void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColours()
    {
        colourGenerator.UpdateColours();
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colourGenerator);
            }
        }
    }

    public void EdgeDetect()
    {
        //NOT IMPLEMENTED
        Debug.Log("NOT IMPLEMENTED");
    }

    public void GetTerrainTexture()
    {

        Material mat = meshFilters[0].GetComponent<MeshRenderer>().material;
        GameObject plane = GameObject.Find("texturePlane");

        Mesh mesh = meshFilters[0].mesh;
        Vector2[] uv = mesh.uv;
        Texture2D tex = new Texture2D(resolution, resolution);
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                int i = x + y * resolution;
                Color color = ((x & y) != 0 ? Color.white : Color.black);
                //Color color = colourSettings.planetMaterial
                int texelX = Mathf.FloorToInt(uv[i].x * resolution);
                int texelY = Mathf.FloorToInt(uv[i].y * resolution);
                tex.SetPixel(texelX, texelY, color);
            }
        }
        tex.Apply();
        //plane.GetComponent<MeshRenderer>().material.mainTexture = tex;
        colourSettings.planetMaterial.SetTexture("_texture", tex);
        //plane.GetComponent<ApplyTexture>().Apply(tex);

        //plane.GetComponent<ApplyTexture>().Apply(tex2);
        /* for (int y = 0; y < resolution; y++)
         {
             for (int x = 0; x < resolution; x++)
             {
                 int i = x + y * resolution;
                 Color color = mesh.colors
                 //int texelX = Mathf.FloorToInt(uv[x].x * resolution);
                 //int texelY = Mathf.FloorToInt(uv[y].y * resolution);
                 fillColorArray[i] = ;
             }
         }*/

        //plane.GetComponent<Renderer>().material = mat;


        //GameObject.Find("texturePlane").GetComponent<ApplyTexture>().Apply(colourGenerator.texture);

        /*Renderer renderer = meshFilters[0].GetComponent<MeshRenderer>();
        foreach (Object obj in EditorUtility.CollectDependencies(new UnityEngine.Object[] { renderer }))
        {
            //Debug.Log(obj);
            if (obj is Material)
            {
                Debug.Log("obj is Material");
                foreach (Object thing in EditorUtility.CollectDependencies(new UnityEngine.Object[] { obj }))
                {
                    Debug.Log(thing);
                }
                //GameObject.Find("texturePlane").GetComponent<ApplyTexture>().Apply(obj as Texture);
            }
        }*/

        //Texture tex = (mat.GetTexture());
        //GameObject.Find("texturePlane").GetComponent<ApplyTexture>().Apply(tex);
        //byte[] bytes = tex.EncodeToPNG();
        //Object.Destroy(tex);
        //Debug.Log(Application.dataPath+"/SavedTexture.png");
        // For testing purposes, also write to a file in the project folder
        //File.WriteAllBytes(Application.dataPath+"/SavedTexture.png", bytes);

        //}
    }
}
