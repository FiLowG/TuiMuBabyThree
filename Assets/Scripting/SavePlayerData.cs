using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavePlayerData : MonoBehaviour
{
    private string taikhoan;
    private string matkhau;
    private string filePath;
    private AuthenManager authenManager;
    private CreateDataFile createfile;
    public GameObject LogIn_PopUp;
    void Awake()
    {
        createfile = FindObjectOfType<CreateDataFile>();
        authenManager = FindObjectOfType<AuthenManager>();
        filePath = Path.Combine(Application.persistentDataPath, "htc.json");
        createfile.CreateFile();

    }
    void Start()
    {


    }
    public void InLog()
    {

        if (File.Exists(filePath) && LogIn_PopUp.activeSelf)
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length >= 2 && !string.IsNullOrEmpty(lines[0]))
            {
                taikhoan = lines[0].Trim();
                matkhau = lines[1].Trim();

                if (authenManager != null)
                {
                    authenManager.NameLogin.text = taikhoan;
                    authenManager.MatKhauLogin.text = matkhau;
                    authenManager.DangNhap();
                    Debug.Log("Login data loaded: " + taikhoan + " / " + matkhau);
                }
                else
                {
                    Debug.LogError("AuthenManager not found in the scene.");
                }
            }
            else
            {
                Debug.Log("File NullOrEmpty.");
            }
        }
        else
        {
            Debug.Log("File not exists or not logining.");
        }
    }
}
