using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class InputScriptWebrequest : MonoBehaviour
{
    private static int MAX_X_INPUT = 20;
    private static int MAX_Z_INPUT = 20;
    private static int MAX_INPUT_MAX = 100;


    private float in_num = 0;
    //public float[,] numbers = new float[MAX_X_INPUT, MAX_Z_INPUT];


    [SerializeField]
    private Button ButtonBT;
    [SerializeField]
    private Button ReadBT;
    [SerializeField]
    private Button RefreshBT;

    [SerializeField]
    public Text DebugText;
    public Text DebugText2;

    [SerializeField]
    public InputField Inputfd;

    [SerializeField]
    public Dropdown dropdx;
    public Dropdown dropdy;
    public Dropdown dropfunc;

    [SerializeField]
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    
    // Start is called before the first frame update
    void Start()
    {
        initDropdown();
        
        //matr_floaturi = new float[MAX_X_INPUT][MAX_Z_INPUT];
        ButtonBT.onClick.AddListener(input_func);
        //ReadBT.onClick.AddListener(read_func);
        //RefreshBT.onClick.AddListener(refresh_func);
        Inputfd.text = "0.0";
        //mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;
        //CreateVector3();
        //CreateShape();
        //UpdateMesh();
    }

    private void initDropdown()
    {
        dropdx.options.Clear();
        dropdy.options.Clear(); ;
        dropfunc.options.Clear();

        for (int i0 = 1; i0 <= MAX_X_INPUT; i0++)
        {
            dropdx.options.Add(new Dropdown.OptionData() { text = i0+" " });
        }
        for (int i1 = 1; i1 <= MAX_Z_INPUT; i1++)
        {
            dropdy.options.Add(new Dropdown.OptionData() { text = i1+" " });
        }
        dropfunc.options.Add(new Dropdown.OptionData() { text = "+" });
        dropfunc.options.Add(new Dropdown.OptionData() { text = "-" });
        dropfunc.options.Add(new Dropdown.OptionData() { text = "x" });
        dropfunc.options.Add(new Dropdown.OptionData() { text = "/" });

        dropdx.value = 0;
        dropdy.value = 0;
        dropfunc.value = 0;
    }


    private void input_func()
    {
        float out_num = 0;
        DebugText.text = Inputfd.text;
        if (!String.IsNullOrEmpty(Inputfd.text))
        {
            try
            {
                in_num = float.Parse(Inputfd.text);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
                DebugText.text = "Not a Number!!!";
                return;
            }
            if (in_num > MAX_INPUT_MAX)
                in_num = MAX_INPUT_MAX;
            else if (in_num < -MAX_INPUT_MAX)
                in_num = -MAX_INPUT_MAX;
            DebugText.text = "Input num: " + in_num + "Total: " + out_num;

            StartCoroutine(sendTextToFile((result) => {
                Debug.Log("result:" + result);
                DebugText.text = result;
            }));
        }
        else
        {
            DebugText.text = "Not a Number!!!";
        }
    }

    IEnumerator sendTextToFile(Action<string> result)
    {
        string command = "input";
        Debug.Log("sendTextToFile");
        DebugText2.text = "sendTextToFile";

        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("input", "" + in_num);
        form.AddField("x", "" + dropdx.value);
        form.AddField("y", "" + dropdy.value);
        form.AddField("function", "" + dropfunc.value);

        // for file basic testing on php
        //using (UnityWebRequest www = UnityWebRequest.Post("https://vincecheng0816.000webhostapp.com/unity/php/requestform.php", form))
        using (UnityWebRequest www = UnityWebRequest.Post("https://vincecheng0816.000webhostapp.com/unity/php/sendform.php", form))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                result(www.error);
            }
            else
            {
                DebugText2.text = www.downloadHandler.text;
                Debug.Log("Form upload complete!"+ www.downloadHandler.text);
                result("Submit success!");
            }
        }

    }
}
