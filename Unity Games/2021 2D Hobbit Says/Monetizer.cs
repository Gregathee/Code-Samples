using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Manages the playing and displaying of advertizements
/// </summary>
public class Monetizer : MonoBehaviour, IUnityAdsListener
{
    public static Monetizer instance;
    string GooglePlay_ID = "4049937";
    string placementId = "bannerPlacement";
    [SerializeField] bool testMode = true;

    public int coinsEarnedThisRound = 0;

    string rewardedPlacemeantId = "rewardedVideo";

    bool adCanBeWatched = true;
    private float timeSinceLastAd = 0;
    private float secondsPerAdd = 60;
    

    void Awake()
    {
        if (!instance) { instance = this; }
        else {Destroy(gameObject);}
    }
    
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(GooglePlay_ID, testMode);
        Advertisement.Banner.SetPosition (BannerPosition.TOP_CENTER);
        StartCoroutine (ShowBannerWhenReady ());
    }

    private void Update()
    {
        // Keep adds from playing to close together in cases of frequent player loss.
        timeSinceLastAd += Time.deltaTime;
        if (timeSinceLastAd > secondsPerAdd) { adCanBeWatched = true; }
    }

    IEnumerator ShowBannerWhenReady () {
        while (!Advertisement.IsReady (placementId)) {
            yield return new WaitForSeconds (0.5f);
        }
        Advertisement.Banner.Show (placementId);
    }

    public void ResetCoins() { coinsEarnedThisRound = 0; }

    /// <summary>
    /// Play one time unrewarded video
    /// </summary>
    public void DisplayInterstitialAd()
    {
        if (adCanBeWatched)
        {
            Advertisement.Show();
            timeSinceLastAd = 0;
            adCanBeWatched = false;
        }
    }

    public void DisplayRewardedAd() { Advertisement.Show(rewardedPlacemeantId); }
    
    public void OnUnityAdsReady(string placementId) { }
    public void OnUnityAdsDidError(string message) { }
    public void OnUnityAdsDidStart(string placementId) { }
    
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
    }
}
