using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class AdsBanner : MonoBehaviour
{
#if UNITY_ANDROID
    private string _bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
    private string _bannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
    private string _bannerAdUnitId = "unused";
#endif

    [Header("Chọn vị trí hiển thị quảng cáo banner")]
    public bool showAtTop = false;
    public bool showAtBottom = false;
    public bool showAtTopLeft = false;
    public bool showAtTopRight = false;
    public bool showAtBottomLeft = false;
    public bool showAtBottomRight = false;
    public bool showAtCenter = false;

    private Dictionary<AdPosition, BannerView> activeBanners = new Dictionary<AdPosition, BannerView>();

    void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;

        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            StartCoroutine(DelayedBannerLoad());
        });
    }

    private IEnumerator DelayedBannerLoad()
    {
        yield return new WaitForSeconds(1f);
        LoadAllSelectedBannerAds();
    }

    public void LoadAllSelectedBannerAds()
    {
        DestroyAllBannerAds();

        TryCreateBanner(showAtTop, AdPosition.Top);
        TryCreateBanner(showAtBottom, AdPosition.Bottom);
        TryCreateBanner(showAtTopLeft, AdPosition.TopLeft);
        TryCreateBanner(showAtTopRight, AdPosition.TopRight);
        TryCreateBanner(showAtBottomLeft, AdPosition.BottomLeft);
        TryCreateBanner(showAtBottomRight, AdPosition.BottomRight);
        TryCreateBanner(showAtCenter, AdPosition.Center);
    }

    private void TryCreateBanner(bool condition, AdPosition position)
    {
        if (!condition) return;

        BannerView banner = new BannerView(_bannerAdUnitId, AdSize.Banner, position);
        var adRequest = new AdRequest();
        banner.LoadAd(adRequest);
        activeBanners.Add(position, banner);

        Debug.Log($"✅ Created banner at position: {position}");
    }

    public void DestroyAllBannerAds()
    {
        foreach (var pair in activeBanners)
        {
            pair.Value.Destroy();
        }
        activeBanners.Clear();
        Debug.Log("🗑️ All banner ads destroyed.");
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        Time.timeScale = 1;
        DestroyAllBannerAds();
        StartCoroutine(DelayedBannerLoad());
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;

        SceneManager.activeSceneChanged -= OnSceneChanged;
        DestroyAllBannerAds();
    }
}
