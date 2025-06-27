using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;


public class BagsFromFirebase : MonoBehaviour
{
    private string UID;
    private DatabaseReference databaseRef;
    private AuthenManager authManager;
    public Text AllBags;
    [HideInInspector]
    public int bagsBounty;

    // Start is called before the first frame update
    void Awake()
    {

        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }
        if (authManager != null)
        {
            UID = authManager.UID.text;
        }
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        _ = GetBagsBounty();

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public async Task GetBagsBounty() 
    { 
        await databaseRef.Child("users").Child(UID).Child("BagsBounty").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Lỗi khi lấy BagsBounty từ database.");
                return;
            }

            if (task.IsCompleted && task.Result.Exists)
            {
                bagsBounty = int.Parse(task.Result.Value.ToString());

            }
            else
            {
                Debug.LogError("Không tìm thấy BagsBounty trong database.");
            }
        });
        AllBags.text = bagsBounty.ToString();
    }
    public void SetBagsBounty(int bagBounty)
    {
        databaseRef.Child("users").Child(UID).Child("BagsBounty").SetValueAsync(bagBounty).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("⚠️ Lỗi khi cập nhật BagsBounty vào database.");
                return;
            }


            // Gọi lại GetValueAsync() sau khi SetValueAsync hoàn thành để lấy giá trị mới
            databaseRef.Child("users").Child(UID).Child("BagsBounty").GetValueAsync().ContinueWithOnMainThread(getTask =>
            {
                if (getTask.IsFaulted || getTask.IsCanceled)
                {
                    Debug.LogError("⚠️ Lỗi khi lấy BagsBounty từ database.");
                    return;
                }

                if (getTask.IsCompleted && getTask.Result.Exists)
                {
                    bagsBounty = int.Parse(getTask.Result.Value.ToString());
                    Debug.Log($"🔄 BagsBounty mới từ database: {bagsBounty}");
                }
                else
                {
                    Debug.LogWarning("⚠️ Không tìm thấy BagsBounty trong database.");
                }
            });
        });
    }

}
