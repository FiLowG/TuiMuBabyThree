using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AuthenManager;

public class Count_Charm_Online : MonoBehaviour
{
    public Text count;
    private SpawnBags_Online spawnBags;
    public GameObject SpawnedList;
    private static bool isSpawning = false; // Biến static để kiểm soát Coroutine
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;
        if (spawnBags == null)
        { 
            spawnBags = FindObjectOfType<SpawnBags_Online>();
        }

        count.text = "0"; // Khởi tạo giá trị ban đầu của text là "0"
        if (this.gameObject.name == "Point_Special")
        {
            count.text = "+0";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(this.gameObject.tag))
        {
            Debug.Log(this.gameObject.tag + "," + other.gameObject.name);
            int currentCount = int.Parse(count.text);
            currentCount++;
            count.text = currentCount.ToString();
            if (this.gameObject.name == "Point_Special")
            {
                UpdateCoin(1);
                count.text = "+" + currentCount.ToString();
            }
            Destroy(other.gameObject);

            if (!isSpawning) // Chỉ chạy nếu chưa có Coroutine nào được chạy
            { 
                StartCoroutine(NewTurnSpawn());
            }
        }
    }
    public void UpdateCoin(int SLCanThem)
    {
        string PriorityRef = "CoinQuantity";
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
        });
    }
    IEnumerator NewTurnSpawn()
    {
        isSpawning = true; // Đánh dấu Coroutine đã được chạy
        yield return new WaitForSeconds(1);

        if (SpawnedList.transform.childCount < 1)
        {
            spawnBags.StartCoroutine(spawnBags.SpawnBagsAndLog());
        }

        isSpawning = false; // Reset lại biến sau khi hoàn thành
    }
}
