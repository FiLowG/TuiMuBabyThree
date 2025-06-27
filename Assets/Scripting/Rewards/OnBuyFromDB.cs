using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OnBuyFromDB : MonoBehaviour
{
    private DatabaseReference dbReference;
    public Text LoaiQua;
    public Text GiaDoi;
    public GameObject FormNhanQua;
    private AuthenManager authManager;
    private string userID;
    public GameObject PopUp_Notice;
    public Text NoticeText;
    public Text NoticeText2;

    // Start is called before the first frame update
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBuy()
    {
        dbReference.Child("RewardsConTrol").Child(this.gameObject.name).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                int price = int.Parse(task.Result.Value.ToString());
                dbReference.Child("users").Child(userID).Child("CoinQuantity").GetValueAsync().ContinueWithOnMainThread(x =>
                {
                    if (x.IsCompleted && x.Result.Exists)
                    {
                        Debug.Log(this.gameObject.name + "CoinQuantity");
                        int userCoin = int.Parse(x.Result.Value.ToString());
                        
                        if (userCoin >= price)
                        {
                            FormNhanQua.SetActive(true);
                            LoaiQua.text = this.gameObject.name;
                            GiaDoi.text = task.Result.Value.ToString();
                        }
                        else if (userCoin < price)
                        {
                            PopUp_Notice.SetActive(true);
                            NoticeText.text = "Bạn không có đủ Charm rồi!";
                            NoticeText2.text = "";
                        }
                    }

                });
            }
            else
            {
                Debug.LogWarning("Không tìm thấy dữ liệu hoặc lỗi kết nối.");
            }
        });
    }

    public void MinusCoin()
    {
        dbReference.Child("users").Child(userID).Child("CoinQuantity").GetValueAsync().ContinueWithOnMainThread(x =>
        {
            if (x.IsCompleted && x.Result.Exists)
            {
                Debug.Log(this.gameObject.name + "CoinQuantity");
                int currentCoin = int.Parse(x.Result.Value.ToString());

                dbReference.Child("RewardsConTrol").Child(this.gameObject.name).GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted && task.Result.Exists)
                    {
                        Debug.Log(int.Parse(x.Result.Value.ToString()) + "Price");
                        int price = int.Parse(task.Result.Value.ToString());
                        dbReference.Child("users").Child(userID).Child("CoinQuantity").SetValueAsync(currentCoin - price);

                    }
                });
                FormNhanQua.SetActive(false);
            }
        });
    }
}
