using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Networking;

public class InputScript : MonoBehaviour
{
    private static int MAX_X_INPUT = 20;
    private static int MAX_Z_INPUT = 20;
    private int z = 0;
    private float[,] numbers = new float[MAX_X_INPUT, MAX_Z_INPUT];


    [SerializeField]
    private Button ButtonBT;
    [SerializeField]
    private Button ResetBT;

    [SerializeField]
    public Text DebugText;
    public Text DebugText2;

    [SerializeField]
    public InputField Inputfd;



    public List<string> inventory = new List<string>();
    public List<string> OnlyX = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //matr_floaturi = new float[MAX_X_INPUT][MAX_Z_INPUT];
        ButtonBT.onClick.AddListener(input_func);
        ResetBT.onClick.AddListener(reset_func);
        Inputfd.text = "0";
        ReadCSVFile();
    }

    private void input_func()
    {
        float out_num = 0;
        DebugText.text = Inputfd.text;
        if (!String.IsNullOrEmpty(Inputfd.text))
        {
            float in_num = float.Parse(Inputfd.text);
            out_num = math_mountain_algorithm(in_num);

            //DebugText.text = "Input num: " + in_num + "Total: " + out_num;
            WriteCSVFile();
            //sendTextToFile(in_num);
        }
    }

    private void reset_func()
    {

        for (int i = 0; i < MAX_X_INPUT; i++)
        {
            for (int j = 0; j < MAX_Z_INPUT; j++)
            {
                numbers[i, j] = 0;
            }
        }
        WriteCSVFile();
        DebugText.text = "Reset Table!!!";
    }


    void ReadCSVFile()
    {
        Debug.Log("InputScript: ReadCSVFile");
        string filePath = getPath();
        DebugText2.text = "getPath():" + filePath;
        StreamReader strReader = new StreamReader(filePath);
        DebugText2.text = "StreamReader():" + filePath;
        bool endoffile = false;
        z = 0;
        while (!endoffile)
        {
            string data_string = strReader.ReadLine();
            if (data_string == null)
            {
                endoffile = true;
                Debug.Log("End of file!!");
                break;
            }
            var data_values = data_string.Split(',');
            for (int i = 0; i < MAX_X_INPUT; i++)
            {
                //Debug.Log("["+z+","+i+"] data: " + data_values[i].ToString());
                numbers[z, i] = float.Parse(data_values[i]);
            }
            z++;
            if (z >= MAX_Z_INPUT)
            {
                endoffile = true;
                Debug.Log("End of file!!");
                break;
            }

        }
        strReader.ReadToEnd();
        strReader.Close();
    }

    void WriteCSVFile() {
        string filePath = getPath();
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < MAX_Z_INPUT; ++i)
            {
                string data_string = "";
                for (int j = 0; j < MAX_X_INPUT; ++j)
                {
                    data_string = data_string + numbers[i, j] + ",";
                }
                writer.WriteLine(data_string);
            }
            writer.Flush();
            writer.Close();
            Debug.Log("write success:" + filePath);
        }
    }
    // Update is called once per frame
    void Update()
    {
        

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

    private float math_mountain_algorithm(float num)
    {
        float smallFloatB = 3.14159f * 3;
        float original = 0;
        float num2 = num;
        int operation = 0; // 0= add, 1=substraction, 2=mutiple, 3=division

        if (num> smallFloatB)
            num2 = num / smallFloatB - (int)(num / smallFloatB);
        //algorithm: 1. decide a location (i, j) (row, column)
        //           2. generate a new number depend on input value and its neighbours 
        // Random location
        int i =  Random.Range(1, MAX_X_INPUT-1);
        int j =  Random.Range(1, MAX_Z_INPUT-1);
        original = numbers[i, j];
        /* 8 neighbours:
         * [i-1, j-1], [i-1, j], [i-1, j+1]
         * [i, j-1],   [i, j],   [i, j+1]
         * [i+1, j-1], [i+1, j], [i+1, j+1]
        */

        // decide a operation:
        operation = ((int)num + i + j) % 4;
        if (operation >= 2) operation = 0;  // temperary disable multiple and division

        // Math Mathod

        float noise = Mathf.PerlinNoise(i *0.3f, j* 0.3f) *0.6f;
        float average = (numbers[i - 1, j - 1] + numbers[i - 1, j] + numbers[i - 1, j + 1] + numbers[i, j - 1] + numbers[i, j + 1] + numbers[i + 1, j - 1] + numbers[i + 1, j] + numbers[i + 1, j + 1]) / 8;
        if (average == 0)
        {
            numbers[i, j] = original + num2 * .3f + noise * .3f;

        }
        else
        {

            if (operation == 0)
            {
                // 1. addition:
                numbers[i, j] = original + noise * num2;
                // neighbours
                numbers[i - 1, j - 1] = numbers[i - 1, j - 1] + (noise * num2 *0.2f);
                numbers[i - 1, j] = numbers[i - 1, j] + (noise * num2 * 0.3f);
                numbers[i - 1, j + 1] = numbers[i - 1, j + 1] + (noise * num2 * 0.2f);
                numbers[i, j - 1] = numbers[i, j - 1] + (noise * num2 * 0.3f);
                numbers[i, j + 1] = numbers[i, j + 1] + (noise * num2 * 0.3f);
                numbers[i + 1, j - 1] = numbers[i + 1, j - 1] + (noise * num2 * 0.2f);
                numbers[i + 1, j] = numbers[i + 1, j] + (noise * num2 * 0.3f);
                numbers[i + 1, j + 1] = numbers[i + 1, j + 1] + (noise * num2 * 0.2f);
            }
            else if (operation == 1)
            {
                // 2. substraction
                numbers[i, j] = original - noise * num2;
            }
            else if (operation == 2)
            {
                // 3. multiple
                float factor = 1 + (num2 * 0.2f);
                numbers[i, j] = (original + noise) * factor;
            }
            else if (operation == 3)
            {
                float factor = 1 + (num2 * 0.2f);
                numbers[i, j] = (original + noise) / factor;
                // 4. division
            }
            else {
                Debug.Log("algorithm failed: unknown opertion" + operation); 
            }
        }


        Debug.Log("algorithm success: ["+ i + "," + j + "] = " + numbers[i, j] + "(" + noise + "," + original + ")");
        DebugText.text = "algorithm success: [" + i + "," + j + "] = " + numbers[i, j] + "(" + noise + "," + original + ")";
        return numbers[i, j];


    }

}
