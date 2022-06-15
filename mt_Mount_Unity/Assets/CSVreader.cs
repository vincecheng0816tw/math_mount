using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CSVreader : MonoBehaviour
{
    private int MAX_X_INPUT = 20;
    private int MAX_Z_INPUT = 20;


    [SerializeField]

    //public GameObject objects;
    //public TextAsset csvObject;
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    [SerializeField]
    public Text DebugText;
    public Text DebugText2;

    // Start is called before the first frame update
    private int x = 0;
    //private int y = 0;
    private int z = 0;

    [SerializeField]
    private Button RefreshBT;

    void Start()
    {
        RefreshBT.onClick.AddListener(refresh_func);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateVector3();
        ReadCSVFile();
        CreateShape();

        UpdateMesh();
    }
    private void CreateVector3()
    {
        Debug.Log("CreateVector3");
        DebugText.text = "CreateVector3";
        vertices = new Vector3[(MAX_X_INPUT + 1) * (MAX_Z_INPUT + 1)];
        for (int i = 0, z = 0; z <= MAX_Z_INPUT; z++)
        {
            for (int x = 0; x <= MAX_X_INPUT; x++)
            {
                //float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
    }

    void CreateShape()
    {
        DebugText.text = "CreateShape";
        int vert = 0;
        int tris = 0;
        triangles = new int[(MAX_Z_INPUT + 1) * (MAX_X_INPUT + 1) * 6];
        for (int az = 0; az < MAX_Z_INPUT; az++)
        {
            for (int ax = 0; ax < MAX_X_INPUT; ax++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + MAX_X_INPUT + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + MAX_X_INPUT + 1;
                triangles[tris + 5] = vert + MAX_X_INPUT + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }
    private void PrintVector3()
    {
        
        for (int i = 0, z = 0; z <= MAX_Z_INPUT; z++)
        {
            for (int x = 0; x <= MAX_X_INPUT; x++)
            {
                //vertices[i] = new Vector3(x, 0, z);
                Debug.Log("PrintVector x:" + vertices[i].x.ToString() + " z: " + vertices[i].z.ToString() + " y: " + vertices[i].y.ToString() );
                i++;
            }
        }
    }

    private void refresh_func()
    {
        DebugText.text = "refresh_func";
        ReadCSVFile();
        //CreateShape();
        UpdateMesh();
    }

    // Update is called once per frame
    void ReadCSVFile() {
        Debug.Log("CSVreader: ReadCSVFile");
        DebugText.text = "CSVreader: ReadCSVFile";
        string filePath = getPath();
        DebugText2.text = "CSVreader: getPath():" + filePath;
        StreamReader strReader1 = new StreamReader(filePath);
        DebugText2.text = "CSVreader:StreamReader():" + filePath;
        bool endoffile =  false;
            
        int index = 0;
        z = 0;
        DebugText2.text = "start while";
        while (!endoffile)
        {
            string data_string = strReader1.ReadLine();
            if (data_string == null)
            {
                endoffile = true;
                Debug.Log("End of file0!!");
                DebugText2.text = "End of file0!!";
                break;
            }
            x = 0;
            
            var data_values = data_string.Split(',');
            //for (int i = 0; i < data_values.Length && i < MAX_Z_INPUT; i
            for (int i = 0; i < MAX_X_INPUT; i++)
            {
                x = i;
                //Debug.Log("x:" + x.ToString() + " z: " + z.ToString() + " data: " + data_values[i].ToString() + " index:" + index.ToString());
                DebugText2.text = "ReadCSVFile: x:" + x.ToString() + " z: " + z.ToString() + " data: " + data_values[i].ToString() + " index:" + index.ToString();
                //float y = Mathf.PerlinNoise(x * .3f, z * .3f) * ((float.Parse(data_values[i])) / 1000);
                float y = ((float.Parse(data_values[i])) / 5f);
                //vertices[index] = new Vector3(x, y , z);
                //vertices[index] = new Vector3(x, ((float.Parse(data_values[i])) / 1000), z);
                //vertices[index] = new Vector3(x, 0 , z);
                vertices[index].Set(x,y,z);
                 
                index++;
            }
            // add one edge line
            {
                //Debug.Log("x:" + MAX_X_INPUT.ToString() + " z: " + z.ToString() + " index:" + index.ToString());
                DebugText2.text = "x:" + MAX_X_INPUT.ToString() + " z: " + z.ToString() + " index:" + index.ToString();
                //vertices[index] = new Vector3(MAX_X_INPUT, 0, z);
                vertices[index].Set(MAX_X_INPUT, 0, z);
                index++;
            }
            z++;
            if (z >= MAX_Z_INPUT)
            {
                endoffile = true;
                //Debug.Log("End of file!!");
                DebugText2.text = "End of file!!";
                for (int i = 0; i <= MAX_X_INPUT; i++)
                {
                    x = i;
                    //Debug.Log("x:" + x.ToString() + " z: " + z.ToString() + " data: " + data_values[i].ToString() + " index:" + index.ToString());
                    //vertices[index] = new Vector3(x, 0, z);
                    vertices[index].Set(x, 0, z);
                    index++;
                }
                DebugText2.text = "x:" + vertices[22].x.ToString() + " z: " + vertices[22].z.ToString() + " y: " + vertices[22].y.ToString();
                strReader1.ReadToEnd();
                strReader1.Close();
                break;
            }

        }
        // print for debug only
        PrintVector3();
        DebugText.text = "CSVreader: ReadCSVFile: done";
    }



    void UpdateMesh() {
        Debug.Log("UpdateMesh");
        //DebugText.text = "UpdateMesh";
        mesh.Clear();
        mesh.RecalculateNormals();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath  + "/"+"maths_mountain.csv";
        //"Participant " + "   " + DateTime.Now.ToString("dd-MM-yy   hh-mm-ss") + ".csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"maths_mountain.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"maths_mountain.csv";
#else
        return Application.dataPath + "/" + "maths_mountain.csv";
#endif
    }
}
