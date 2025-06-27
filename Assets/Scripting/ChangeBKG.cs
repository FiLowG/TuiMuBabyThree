using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeBKG : MonoBehaviour
{
    private string filePath;

    public GameObject ChangePOPUP;
    public GameObject Type_BKG;
    
    public GameObject OnSetting1, OnSetting2, OnSetting3, OnSetting4, OnSetting5;
    public GameObject BackGround1, BackGround2, BackGround3, BackGround4, BackGround5;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "htc.json");
        if (File.Exists(filePath))
        {

        }
            if (this.gameObject.name == "BG_FULL")
        {
            StartChangeBKG();
        }
    }
    public void StartChangeBKG()
    {
        ChangePOPUP.SetActive(false);
        string btnName = Type_BKG.tag;

        switch (btnName)
        {
            case "BKG1":
                SetActiveSet(1);
                Type_BKG.tag = "BKG1";
                break;
            case "BKG2":
                SetActiveSet(2);
                Type_BKG.tag = "BKG2";
                break;
            case "BKG3":
                SetActiveSet(3);
                Type_BKG.tag = "BKG3";
                break;
            case "BKG4":
                SetActiveSet(4);
                Type_BKG.tag = "BKG4";
                break;
            case "BKG5":
                SetActiveSet(5);
                Type_BKG.tag = "BKG5";
                break;
        }
    }
    public void OnClickChangeBackground(GameObject button)
    {
        ChangePOPUP.SetActive(false);
        string btnName = button.name;

        switch (btnName)
        {
            case "BKG1":
                SetActiveSet(1);
                Type_BKG.tag = "BKG1";
                break;
            case "BKG2":
                SetActiveSet(2);
                Type_BKG.tag = "BKG2";
                break;
            case "BKG3":
                SetActiveSet(3);
                Type_BKG.tag = "BKG3";
                break;
            case "BKG4":
                SetActiveSet(4);
                Type_BKG.tag = "BKG4";
                break;
            case "BKG5":
                SetActiveSet(5);
                Type_BKG.tag = "BKG5";
                break;
        }
        WriteBackgroundSetting(btnName);
    }
    void WriteBackgroundSetting(string btnName)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length < 6)
        {
            System.Array.Resize(ref lines, 6);
        }

        if (currentScene == "Bounty_MODE")
        {
            lines[3] = btnName; // Ghi vào dòng 4 (chỉ mục 3)
        }
        else if (currentScene == "Normal_Play")
        {
            lines[2] = btnName; // Ghi vào dòng 3 (chỉ mục 2)
        }

        File.WriteAllLines(filePath, lines);
    }

void SetActiveSet(int index)
    { 
        OnSetting1.SetActive(false);
        OnSetting2.SetActive(false);
        OnSetting3.SetActive(false);
        OnSetting4.SetActive(false);
        if (OnSetting5 != null)
        {
            OnSetting5.SetActive(false);

        }

        BackGround1.SetActive(false);
        BackGround2.SetActive(false);
        BackGround3.SetActive(false);
        BackGround4.SetActive(false);
        if (BackGround5 != null)
        {
            BackGround5.SetActive(false);
        }

        // Kích hoạt theo chỉ số được chọn
        switch (index)
        {
            case 1:
                OnSetting1.SetActive(true);
                BackGround1.SetActive(true);
                break;
            case 2:
                OnSetting2.SetActive(true);
                BackGround2.SetActive(true);
                break;
            case 3:
                OnSetting3.SetActive(true);
                BackGround3.SetActive(true);
                break;
            case 4:
                OnSetting4.SetActive(true);
                BackGround4.SetActive(true);
                break;
            case 5:
                if (OnSetting5 != null && BackGround5 != null)
                {
                    OnSetting5.SetActive(true);
                    BackGround5.SetActive(true);
                }
                break;
        }
    }
}
