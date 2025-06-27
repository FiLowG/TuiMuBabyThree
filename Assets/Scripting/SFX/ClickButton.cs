using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    public AudioSource audioSource;

    // Gọi hàm này mỗi khi muốn phát âm thanh
    public void PlayClickSound(AudioClip clickSound)
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
