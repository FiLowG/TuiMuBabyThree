using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase.Database;
using static AuthenManager;
using Firebase.Extensions;

public class AdsRewarded : MonoBehaviour
{
#if UNITY_ANDROID
    private string _rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917"; // Test rewarded
#elif UNITY_IPHONE
    private string _rewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _rewardedAdUnitId = "unused";
#endif
    private DatabaseReference databaseRef;
    private AuthenManager authManager;

    private RewardedAd _rewardedAd;
    private string UID;
    public Text AllBags;
    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadRewardedAd();
        });
        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        UID = authManager.UID.text;
    }

    public void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("📥 Loading Rewarded Ad...");
        var adRequest = new AdRequest();

        RewardedAd.Load(_rewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("❌ Failed to load rewarded ad: " + error);
                return;
            }

            Debug.Log("✅ Rewarded ad loaded.");
            _rewardedAd = ad;
            RegisterRewardedAdEvents(_rewardedAd);
        });
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"🎁 User rewarded! Type: {reward.Type}, Amount: {reward.Amount}");
                OnRewarded(); // Tặng quà
            });
        }
        else
        {
            Debug.Log("⚠️ Rewarded ad not ready. Reloading...");
            LoadRewardedAd();
        }
    }

    private void RegisterRewardedAdEvents(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("📪 Rewarded ad closed. Reloading...");
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("❌ Failed to show rewarded ad: " + error);
        };

        ad.OnAdClicked += () => Debug.Log("🖱️ Rewarded ad clicked.");
        ad.OnAdImpressionRecorded += () => Debug.Log("📊 Impression recorded.");
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"💰 Ad paid {adValue.Value} {adValue.CurrencyCode}");
        };
    }

    private void OnRewarded()
    {
        // ✨ Tặng thưởng cho người chơi
        Debug.Log("💎 Tặng người chơi 16 túi!");
        // PlayerData.coins += 100; // Gọi logic của bạn
        UpdateBagsNormal(16);
    }
    public void UpdateBagsNormal(int addmore)
    {
        string path = "users/" + UID + "/" + "BagsNormal";

        // Lấy giá trị hiện tại từ Firebase
        databaseRef.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    int currentValue = Convert.ToInt32(snapshot.Value);
                    int newValue = currentValue + addmore;

                    // Cập nhật UI an toàn vì đang ở main thread
                    AllBags.text = newValue.ToString();

                    // Ghi lại giá trị mới vào Firebase
                    databaseRef.Child(path).SetValueAsync(newValue);
                }
            }
        });
    }

}
