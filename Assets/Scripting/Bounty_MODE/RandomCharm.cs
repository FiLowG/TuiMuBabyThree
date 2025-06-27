using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;

public class RandomCharm : MonoBehaviour
{
    private string UID;
    private DatabaseReference databaseRef;
    private AuthenManager authManager;

    public GameObject Labubu_Special;
    public GameObject Turtle_Special;
    public GameObject Lububu_Special;
    public GameObject Type_Charm;
    public int valuerate;


    void Start()
    {
        authManager = FindObjectOfType<AuthenManager>();
        if (authManager != null)
        {
            UID = authManager.UID.text;
        }

        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(UpdateRareRateRoutine());;


    }

    void Update()
    {
        Debug.Log($"Current rareRate value: {valuerate}");
    }

    private async Task GetRareRateValue()
    {
        if (string.IsNullOrEmpty(UID))
        {
            return;
        }

        await databaseRef.Child("users").Child(UID).Child("RareRate").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Lỗi khi lấy rareRate từ database.");
                return;
            }

            if (task.IsCompleted && task.Result.Exists)
            {
                valuerate = int.Parse(task.Result.Value.ToString());

            }
            else
            {
                Debug.LogError("Không tìm thấy rareRate trong database.");
            }
        });
    }
   
   
    /// <summary>
    /// Coroutine để cập nhật giá trị rareRate mỗi giây.
    /// </summary>
    private IEnumerator UpdateRareRateRoutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            _ = GetRareRateValue();
        }
    }
    
    /// <summary>
    /// Kiểm tra rareRate trên database và trả về GameObject charm hiếm hoặc charm thường.
    /// </summary>
    public async Task<GameObject> GetRandomCharm()
    {
        await GetRareRateValue();
        if (valuerate <= 0)
        {
            int newRareRate = Random.Range(200, 301);
            await databaseRef.Child("users").Child(UID).Child("RareRate").SetValueAsync(newRareRate);
            return GetSpecialCharm();

        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Trả về charm hiếm dựa theo Type_Charm.
    /// </summary>
    private GameObject GetSpecialCharm()
    {

        switch (Type_Charm.tag)
        {
            case "Labubu":
                return Labubu_Special;
            case "Turtle":
                return Turtle_Special;
            case "Lububu":
                return Lububu_Special;
            default:
                Debug.LogWarning("Không xác định được loại charm đặc biệt!");
                return null;
        }
    }

 
}
