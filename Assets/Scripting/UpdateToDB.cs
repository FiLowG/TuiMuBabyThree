using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateToDB : MonoBehaviour
{
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;
    private string path;
    public string Priority;
    public Text TextCount;
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;

        path = "users/" + userID + "/" + Priority;
        UpdateCoin();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateCoin()
    {
        
        dbReference.Child(path).GetValueAsync().ContinueWith(task =>
        {
            int SLHientai = int.Parse(task.Result.Value.ToString());
            int SlCanThem = int.Parse(TextCount.text);
            dbReference.Child(path).SetValueAsync(SLHientai + SlCanThem);
        });
       
        TextCount.gameObject.GetComponent<Text>().enabled = false;
        TextCount.gameObject.GetComponent<Text>().enabled = true;

    }


    public void UpdateCoinRef(int SLCanThem)
    {
        string PriorityRef = "BagsNormal";
        string pathRef = "users/" + userID + "/" + PriorityRef;
        dbReference.Child(pathRef).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Lỗi khi lấy dữ liệu từ Firebase: " + task.Exception);
                return;
            }

            if (task.Result != null && task.Result.Value != null)
            {
                int SLHientai = int.Parse(task.Result.Value.ToString());
                dbReference.Child(pathRef).SetValueAsync(SLHientai + SLCanThem);
            }
            else
            {
                Debug.LogWarning("Dữ liệu chưa tồn tại tại: " + pathRef + " → tạo mới giá trị");
                dbReference.Child(pathRef).SetValueAsync(SLCanThem);
            }
        });
    }

}
