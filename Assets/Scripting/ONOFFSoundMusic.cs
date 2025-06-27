using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ONOFFSoundMusic : MonoBehaviour
{
    private string filePath;
    public GameObject ALLMusic;
    public GameObject OFFMusic_ICON;
    public GameObject ALLSound;
    public GameObject OFFSound_ICON;
    public GameObject OnSoundBTN;
    public GameObject ONMusicBTN;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "htc.json");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length >= 6)
            {
                if (lines[4].Trim() == "OFF")
                {
                    Debug.Log("Music dang OFF");
                    OFFMusic_ICON.SetActive(true);
                    ALLMusic.SetActive(false);
                    ONMusicBTN.SetActive(false);
                    

                }
                else if (lines[4].Trim() == "ON")
                {
                    Debug.Log("Music dang ON");

                    OFFMusic_ICON.SetActive(false);
                    ALLMusic.SetActive(true);
                    ONMusicBTN.SetActive(true);
                   
                }

                if (lines[5].Trim() == "OFF")
                {
                    Debug.Log("Sound dang OFF");

                    OFFSound_ICON.SetActive(true);
                    ALLSound.SetActive(false);
                    OnSoundBTN.SetActive(false);
                    

                }
                else if (lines[5].Trim() == "ON")
                {
                    Debug.Log("Sound dang ON");

                    OFFSound_ICON.SetActive(false);
                    ALLSound.SetActive(true);
                    OnSoundBTN.SetActive(true);

                }
            }
        }
    }

    public void OnMusic()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length >= 6)
            {
                lines[4] = "ON";
                File.WriteAllLines(filePath, lines);

                OFFMusic_ICON.SetActive(false);
                ALLMusic.SetActive(true);
                ONMusicBTN.SetActive(true);
            }
        }
    }

    public void OFFMusic()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length >= 6)
            {
                lines[4] = "OFF";
                File.WriteAllLines(filePath, lines);

                OFFMusic_ICON.SetActive(true);
                ALLMusic.SetActive(false);
                ONMusicBTN.SetActive(false);
            }
        }
    }

    public void OnSound()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length >= 6)
            {
                lines[5] = "ON";
                File.WriteAllLines(filePath, lines);

                OFFSound_ICON.SetActive(false);
                ALLSound.SetActive(true);
                OnSoundBTN.SetActive(true);
                
            }
        }
    }

    public void OFFSound()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length >= 6)
            {
                lines[5] = "OFF";
                File.WriteAllLines(filePath, lines);

                OFFSound_ICON.SetActive(true);
                ALLSound.SetActive(false);
                OnSoundBTN.SetActive(false);
            }
        }
    }
}
