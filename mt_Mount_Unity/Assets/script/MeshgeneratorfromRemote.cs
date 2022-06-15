using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MeshgeneratorfromRemote : MonoBehaviour
{
    private int MAX_X_INPUT = 20;
    private int MAX_Z_INPUT = 20;
    private float Y_SCALE_COF = 20f;
    private float rotSpeed = 10;
    private Color[] colors;

    private float minTerrainHeight = 0;
    private float maxTerrainHeight = 0;

    [SerializeField]

    //public GameObject objects;
    //public TextAsset csvObject;
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    [SerializeField]
    public Text DebugText;
    [SerializeField]
    public Text DebugText2;


    [SerializeField]
    private Button RefreshBT;

    [SerializeField]
    public Gradient gradient;


    void Start()
    {
        RefreshBT.onClick.AddListener(refresh_func);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateVector3();
        CreateShape();
        //PrintVector3();
        
        StartCoroutine(getTextFromFile((result) => {
            Debug.Log("result:" + result);
            DebugText.text = result;
            UpdateMesh();
        }));
        
    }
    private void CreateVector3()
    {
        Debug.Log("CreateVector3");
        DebugText.text = "CreateVector3";
        vertices = new Vector3[(MAX_X_INPUT) * (MAX_Z_INPUT)];
        for (int i = 0, z = 0; z < MAX_Z_INPUT; z++)
        {
            for (int x = 0; x < MAX_X_INPUT; x++)
            {
                //float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
        colors = new Color[vertices.Length];
     
    }

    void CreateShape()
    {
        DebugText.text = "CreateShape";
        int vert = 0;
        int tris = 0;
        triangles = new int[(MAX_Z_INPUT -1) * (MAX_X_INPUT-1) * 6];
        for (int az = 0; az < MAX_Z_INPUT-1; az++)
        {
            for (int ax = 0; ax < MAX_X_INPUT-1; ax++)
            {
                //Debug.Log("tris: "+ tris+ " vert: "+ vert+ " az: "+az+" ax: "+ax);
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + MAX_X_INPUT ;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + MAX_X_INPUT ;
                triangles[tris + 5] = vert + MAX_X_INPUT + 1;
                vert++;
                tris += 6;
                
            }
            vert++;
        }
        
    }
    private void PrintVector3()
    {
        Debug.Log("Number of vertices: " + vertices.Length + " Number of triangles: " + triangles.Length);

        for (int tris = 0,  z = 0; z < MAX_Z_INPUT-1; z++)
        {
            for (int x = 0; x < MAX_X_INPUT-1; x++)
            {
                //Debug.Log("triangles[0~5]: "+ triangles[tris + 0]+" "+ triangles[tris + 1]+" "+ triangles[tris + 2]+" "+triangles[tris + 3] + " " + triangles[tris + 4] + " " + triangles[tris + 5]);
                tris += 6;
            }
        }

        for (int i = 0, z = 0; z < MAX_Z_INPUT; z++)
        {
            for (int x = 0; x < MAX_X_INPUT; x++)
            {
                //vertices[i] = new Vector3(x, 0, z);
                Debug.Log("PrintVector x:" + vertices[i].x.ToString() + " z: " + vertices[i].z.ToString() + " y: " + vertices[i].y.ToString());
                i++;
            }
        }

        
    }

    private void refresh_func()
    {
        DebugText.text = "refresh_func";
        StartCoroutine(getTextFromFile((result) => {
            Debug.Log("result:" + result);
            DebugText.text = result;
            UpdateMesh();
        }));
    }

    IEnumerator getTextFromFile(Action<string> result)
    {
        WWWForm form = new WWWForm();
        // localhost for testing
        //using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:9900/requestform.php", form))

        using (UnityWebRequest www = UnityWebRequest.Post("https://vincecheng0816.000webhostapp.com/unity/php/readForm.php", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                result(www.error);
            }
            else
            {
                Debug.Log(www.downloadedBytes);
                Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                //byte[] results = www.downloadHandler.data;
                parseTextToNumber(www.downloadHandler.text);
                result("Download complete!");
                //result(www.downloadHandler.text);
            }
        }
    }

    // real location in vertices is different from csv file; return -1 if (x,z) is out of range
    int getIndex(int az, int ax) {
        int index0 = 0;
        if((ax >= MAX_X_INPUT) || (az >= MAX_Z_INPUT))
            return -1;
        index0 = ax  + (az * MAX_X_INPUT);
        return index0;
    }

    void parseTextToNumber(string text)
    {
        
        int x = 0;
        int z = 0;
        string[] subs = text.Split('*');
        foreach (var sub in subs) // loop for parse row
        {
            Debug.Log("subs: " + sub + "\n");

            if (sub != null) {
                string[] sub2 = sub.Split(',');
                x = 0;
                foreach (var inputN in sub2) {
                    //Debug.Log("inputN: " + inputN );
                    if ((inputN != "") || (inputN != "\n") || (inputN != " ")) {
                        int inputF = 0;
                        try
                        {
                            inputF = (int.Parse(inputN, System.Globalization.NumberStyles.HexNumber));
                            float y = ((float)inputF) / Y_SCALE_COF;
                            //float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                            int index = getIndex(z, x);
                            //Debug.Log("inputXYZ: " + x + ", " + y + " " + z + " index:" + index+" org: " + vertices[index]);
                            if (index>=0)
                                vertices[index].Set(x, y, z);
                            if (y > maxTerrainHeight)
                                maxTerrainHeight = y;
                            if (y < minTerrainHeight)
                                minTerrainHeight = y;
                            Debug.Log("1inputF: " + y + ", " + z + " " + x + " index:" + index);
                            x++;
                        }
                        catch (Exception e)
                        {
                            Debug.Log("error: ");
                            Debug.LogException(e, this);
                        }
                    }
                    if (x >= MAX_X_INPUT) {
                        break;
                    }
                }
                z++;
                if (z >= MAX_Z_INPUT)
                    break;
            }
        } // loop for parse row

        for (int i = 0, az = 0; az < MAX_Z_INPUT; az++)
        {
            for (int ax = 0; ax < MAX_X_INPUT; ax++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);


                i++;
            }
        }

    }


    void UpdateMesh()
    {
        Debug.Log("UpdateMesh");
        //DebugText.text = "UpdateMesh";
        mesh.Clear();
        mesh.RecalculateNormals();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
    }

    //void OnMouseDrag()
    //{
    //    float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
    //    float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
    //    //transform.position = GetMouseWorldPos() + mOffset;
    //    transform.RotateAround(Vector3.down, -rotX);
    //    transform.RotateAround(Vector3.right, -rotY);
    //    Debug.Log("OnMouseDrag");
    //}

}
