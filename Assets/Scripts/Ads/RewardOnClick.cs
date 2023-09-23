using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardOnClick : MonoBehaviour,IUnityAdsShowListener, IUnityAdsLoadListener
{
    [SerializeField] private string _androidAdUnityId = "Rewarded_Android";
    [SerializeField] private string _iosAdUnityId = "Rewarded_iOS";
    [SerializeField] private TimeoutWindow timeout;

    private string _adUnityId;


    public void LoadAd()
    {
        Advertisement.Load(_adUnityId, this);
    }

    public void ShowAd()
    {
        Advertisement.Show(_adUnityId, this);
    }
    public void OnUnityAdsShowComplete(string adUnityId, UnityAdsShowCompletionState showCompletionState)
    {

        if (adUnityId.Equals(_adUnityId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            timeout.AddTime();
        }
    }
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("OnUnityAdsAdLoaded");

    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("OnUnityAdsFailedToLoad");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("OnUnityAdsShowClick");
    }


    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("OnUnityAdsShowFailure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("ads show finish");
        Debug.Log("ads show finish");
    }
    private void Awake()
    {
        _adUnityId = (Application.platform == RuntimePlatform.IPhonePlayer)
        ? _iosAdUnityId
        : _androidAdUnityId;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadAd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
