using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LogOut : MonoBehaviour
{
    private string filePath;
    // Start is called before the first frame update
    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "htc.json");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Logout()
    {
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length >= 2)
        {
            lines[0] = "";  // Xóa nội dung dòng đầu tiên
            lines[1] = "";  // Xóa nội dung dòng thứ hai
        }
        File.WriteAllLines(filePath, lines);
        Debug.Log("Cleared login data.");
        SceneManager.LoadScene("LoginOut");
    }
}
