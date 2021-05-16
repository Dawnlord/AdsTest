using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    public string interstitialAdsId;
    public string rewardAdsId;
    public string callbackSecretKey;

    public Button loadInterstitialButton;
    public Button loadRewardButton;
    public Button showInterstitialButton;
    public Button showRewardButton;
    public Text rewardCount;

    private bool initialed = false;
    private int count;

    public void InitialMoPub()
    {
        if (!initialed)
        {
            MoPub.SdkConfiguration config = new MoPub.SdkConfiguration
            {
                AdUnitId = MoPubManager.Instance.AdUnitId,
                AllowLegitimateInterest = MoPubManager.Instance.AllowLegitimateInterest,
                LogLevel = MoPubManager.Instance.LogLevel,
                MediatedNetworks = MoPubManager.Instance.GetComponents<MoPubNetworkConfig>().Where(nc => nc.isActiveAndEnabled).Select(nc => nc.NetworkOptions).ToArray()
            };

            MoPub.InitializeSdk(config);

            MoPub.LoadInterstitialPluginsForAdUnits(new string[1] { interstitialAdsId });
            MoPub.LoadRewardedVideoPluginsForAdUnits(new string[1] { rewardAdsId });

            MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoaded;
            MoPubManager.OnRewardedVideoLoadedEvent += OnRewardLoaded;

            MoPubManager.OnInterstitialShownEvent += OnInterstitialShown;
            MoPubManager.OnRewardedVideoShownEvent += OnRewardShown;

            MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardComplete;

            initialed = true;
            count = 0;
            rewardCount.text = "Reward Count: " + count.ToString();
        }
    }

    public void LoadInterstitialAds()
    {
        MoPub.RequestInterstitialAd(interstitialAdsId);
    }

    public void LoadRewardAds()
    {
        MoPub.RequestRewardedVideo(rewardAdsId);
    }

    private void OnInterstitialLoaded(string interstitialAdsId)
    {
        loadInterstitialButton.interactable = false;
        showInterstitialButton.interactable = true;
        Debug.Log("Interstitial Ads Loaded");
    }

    private void OnRewardLoaded(string rewardAdsId)
    {
        loadRewardButton.interactable = false;
        showRewardButton.interactable = true;
        Debug.Log("Reward Ads Loaded");
    }

    public void ShowInterstitialAds()
    {
        MoPub.ShowInterstitialAd(interstitialAdsId);
    }

    public void ShowRewardAds()
    {
        MoPub.ShowRewardedVideo(rewardAdsId);
    }

    private void OnInterstitialShown(string interstitialAdsId)
    {
        loadInterstitialButton.interactable = true;
        showInterstitialButton.interactable = false;
    }

    private void OnRewardShown(string rewardAdsId)
    {
        loadRewardButton.interactable = true;
        showRewardButton.interactable = false;
    }

    private void OnRewardComplete(string rewardAdsId, string label, float amount)
    {
        count++;
        rewardCount.text = "Reward Count: " + count.ToString();
    }
}
