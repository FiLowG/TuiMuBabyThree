using UnityEngine;
using Firebase.Database;
using System;
using System.Collections;
using Firebase.Extensions;
using UnityEngine.UI;

public class PurchaseDone : MonoBehaviour
{
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;  // Thay bằng UID thực tế của người dùng
    public GameObject Notice_Panel;
    public Text Notice_Text;
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;

    }


    // Lấy giá trị hiện tại từ Firebase
    public void BuySpecialBagPack()
    {
        string pathDiamonds = "users/" + userID + "/DiamondQuantity";
        string pathBagsBounty = "users/" + userID + "/BagsBounty";
        string pathBagsNormal = "users/" + userID + "/BagsNormal";

        // Lấy số kim cương hiện tại
        dbReference.Child(pathDiamonds).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    int currentDia = Convert.ToInt32(snapshot.Value);
                    if (currentDia >= 499)
                    {
                        // Trừ 499 Diamond
                        dbReference.Child(pathDiamonds).SetValueAsync(currentDia - 499).ContinueWithOnMainThread(task1 =>
                        {
                            if (task1.IsCompleted)
                            {
                                // Cộng 100 BagsBounty
                                dbReference.Child(pathBagsBounty).GetValueAsync().ContinueWithOnMainThread(task2 =>
                                {
                                    if (task2.IsCompleted)
                                    {
                                        int currentBounty = task2.Result.Exists ? Convert.ToInt32(task2.Result.Value) : 0;
                                        dbReference.Child(pathBagsBounty).SetValueAsync(currentBounty + 100).ContinueWithOnMainThread(task3 =>
                                        {
                                            if (task3.IsCompleted)
                                            {
                                                // Cộng 30 BagsNormal
                                                dbReference.Child(pathBagsNormal).GetValueAsync().ContinueWithOnMainThread(task4 =>
                                                {
                                                    if (task4.IsCompleted)
                                                    {
                                                        int currentNormal = task4.Result.Exists ? Convert.ToInt32(task4.Result.Value) : 0;
                                                        dbReference.Child(pathBagsNormal).SetValueAsync(currentNormal + 30).ContinueWithOnMainThread(task5 =>
                                                        {
                                                            if (task5.IsCompleted)
                                                            {
                                                                Debug.Log("Mua thành công: -499 Diamond, +100 Bounty, +30 Normal");
                                                                Notice_Panel.SetActive(true);
                                                                Notice_Text.text = "Giao dịch thành công!";
                                                            }
                                                            else
                                                            {
                                                                Debug.LogError("Không thể cộng BagsNormal.");
                                                            }
                                                        });
                                                    }
                                                });
                                            }
                                            else
                                            {
                                                Debug.LogError("Không thể cộng BagsBounty.");
                                            }
                                        });
                                    }
                                });
                            }
                            else
                            {
                                Debug.LogError("Không thể trừ Diamond.");
                            }
                        });
                    }
                    else
                    {
                        Debug.LogWarning("Không đủ kim cương để mua.");
                        Notice_Panel.SetActive(true);
                        Notice_Text.text = "Bạn không đủ kim cương mất rồi!";
                    }
                }
                else
                {
                    Debug.LogError("Không tìm thấy dữ liệu Diamond.");
                }
            }
            else
            {
                Debug.LogError("Lỗi khi truy xuất dữ liệu Diamond từ Firebase.");
            }
        });
    }
}

   