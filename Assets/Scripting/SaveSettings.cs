using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSettings : MonoBehaviour
{
    private string filePath;

    public GameObject Type_BKGNormal;
    public GameObject Type_BKGBounty;
    /*public GameObject MusicOFF;
    public GameObject SoundOFF;*/

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "htc.json");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            /*if (lines.Length >= 6)
            {
                MusicOFF.SetActive(lines[4].Trim() == "OFF");
                SoundOFF.SetActive(lines[5].Trim() == "OFF");
            }*/

            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "Bounty_MODE" && lines.Length >= 4)
            {
                Type_BKGBounty.tag = lines[3].Trim();
            }
            else if (currentScene == "Normal_Play" && lines.Length >= 3)
            {
                Type_BKGNormal.tag = lines[2].Trim();
            }
        }
        else
        {
            Debug.LogWarning("File htc.json không tồn tại.");
        }
    }
}
