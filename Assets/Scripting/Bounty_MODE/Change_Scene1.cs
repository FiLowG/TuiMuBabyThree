using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script sử dụng để chuyển scene.
/// </summary>
public class Change_Scene_Online : MonoBehaviour
{
    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
