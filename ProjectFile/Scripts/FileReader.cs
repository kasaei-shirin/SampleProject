using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FileReader : MonoBehaviour
{
    //variables
    Ray ray;
    RaycastHit hit;
    public GameObject prefabfileObject, barChart, fileText, barText;
    private IEnumerator coroutine;
    private List<List<rowItem>> masterData = new List<List<rowItem>>();
    float distance;
    bool fileLoded = false;
    string path = null;
    public TMP_InputField Username_field;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("rootCheck_interval",10f,10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
            Debug.Log("Close");
        }
        StartCoroutine(rootCheck());

       
    }

    private void rootCheck_interval()
    {
        if(fileLoded)
            chartProducer(path);

    }
        private IEnumerator rootCheck()
    {
       

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
                if (hit.collider.tag == "runForm")
                {
#if UNITY_EDITOR
                    path = EditorUtility.OpenFolderPanel("Open chart data", "", "csv");
#else
                    path = Username_field.text;
                    Debug.Log(path);
#endif

                    if (path.Length != 0)
                    {
                        chartProducer(path);
                        fileLoded = true;
                    }
                }
        }
        yield return new WaitForSeconds(0);
           
    }


    void DestroyWithTag(string destroyTag)
    {
        GameObject[] destroyObject;
        destroyObject = GameObject.FindGameObjectsWithTag(destroyTag);
        foreach (GameObject oneObject in destroyObject)
            Destroy(oneObject);
    }

    public void chartProducer(string path) {
        try
        {
            DestroyWithTag("fileComponent");
        }
        catch { }
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] AllFiles = dir.GetFiles("*.csv");
        for (int i_counter = 0; i_counter < AllFiles.Length; i_counter++)
        {
            var fileContent = File.ReadAllLines(AllFiles[i_counter].FullName);
            var fileName = Path.GetFileName(AllFiles[i_counter].FullName);
            FileController fileController = new FileController();
            var list_currentFile = fileController.getPrecent(fileContent);
            var list_currentFile_origin = fileController.convert2list(fileContent);
            masterData.Add(list_currentFile);
            // Object instanciate
            GameObject fileH = Instantiate(prefabfileObject, new Vector3(0, (3.5f * i_counter), (2 * i_counter)), Quaternion.identity);
            GameObject fileT = Instantiate(fileText, fileH.transform.position, Quaternion.identity);
            fileT.GetComponentInChildren<TextMeshPro>().text = fileName;
            // use current avereage file
            {
                float _max = fileController.getMax(fileContent);
                for (int j = 0; j < list_currentFile.Count; j++)
                {

                    GameObject barScale = Instantiate(barChart, new Vector3(fileH.transform.position.x + j, fileH.transform.position.y + 1, fileH.transform.position.z), Quaternion.identity);
                    GameObject barScaleT = Instantiate(barText, barScale.transform.position, Quaternion.identity);
                    barScaleT.GetComponentInChildren<TextMeshPro>().text = ((int)(list_currentFile_origin[j].value)).ToString();
                    distance = +fileH.transform.position.x + j;
                    barScale.transform.localScale = new Vector3(barScale.transform.localScale.x, list_currentFile[j].value, barScale.transform.localScale.z);

                    if (list_currentFile[j].value <= 0.33)
                        barScale.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.blue);

                    if (list_currentFile[j].value > 0.33 && list_currentFile[j].value <= 0.66)
                        barScale.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.green);

                    if (list_currentFile[j].value > 0.66)
                        barScale.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);


                }
                fileH.transform.localScale = new Vector3(fileH.transform.localScale.x + distance, fileH.transform.localScale.y, fileH.transform.localScale.z);
                fileH.transform.position = new Vector3(fileH.transform.position.x - .55f, fileH.transform.position.y, fileH.transform.position.z);
                //list_currentFile.Count
            }

        }

        foreach (var file in masterData)
        {
            foreach (var lineData in file)
            {
                Debug.Log(lineData.description + "____" + lineData.value);
            }
            Debug.Log("##########################################");
        }

    }
}