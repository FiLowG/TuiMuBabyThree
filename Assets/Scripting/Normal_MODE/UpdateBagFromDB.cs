using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class UpdateBagFromDB : MonoBehaviour
{
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;
    private string path;
    public string Priority;
    public Text BagsCountText;
    // Start is called before the first frame update
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;

        path = "users/" + userID + "/" + "BagsNormal";
        UpdateBags();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateBags()
    {
        dbReference.Child(path).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Lỗi khi lấy dữ liệu từ Firebase: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    BagsCountText.text = task.Result.Value.ToString();
                    Debug.Log("Số túi: " + task.Result.Value);
                }
                else
                {
                    BagsCountText.text = "0";
                    Debug.Log("Dữ liệu không tồn tại.");
                }
            }
                       
        });
        StartCoroutine(WaitUpdate());

    }

    IEnumerator WaitUpdate()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateBags();
        
    }

}
