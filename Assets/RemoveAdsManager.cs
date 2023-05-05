using UnityEngine;
using static Oracle;

public class RemoveAdsManager : MonoBehaviour
{
    [SerializeField] private GameObject adButton;
    [SerializeField] private GameObject removedAdButton;

    private void Awake()
    {
        adButton.SetActive(!oracle.saveData.preferences.removeAds);
        removedAdButton.SetActive(oracle.saveData.preferences.removeAds);
    }
}