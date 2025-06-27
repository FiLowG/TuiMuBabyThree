using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreateDataFile : MonoBehaviour
{
    private string filePath;

    // Start is called before the first frame update
    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "htc.json");
        if (!File.Exists(filePath))
        {
            string[] defaultContent = { "", "", "BKG1", "BK5","ON","ON" };
            File.WriteAllLines(filePath, defaultContent);
            Debug.Log("File htc.json created with default content.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateFile()
    {
        filePath = Path.Combine(Application.persistentDataPath, "htc.json");
        if (!File.Exists(filePath))
        {
            string[] defaultContent = { "", "", "BKG1", "BK5", "ON", "ON" };
            File.WriteAllLines(filePath, defaultContent);
            Debug.Log("File htc.json created with default content.");
        }
    }
}
