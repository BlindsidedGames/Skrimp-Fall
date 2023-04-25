using System;
using UnityEngine;
#if UNITY_IOS
// Include the IosSupport namespace if running on iOS:
using Unity.Advertisement.IosSupport;
#endif

public class AttPermissionRequest : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_IOS
        RequestAuthorizationTracking();
        // Check the user's consent status.
        // If the status is undetermined, display the request request:
        if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED) {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif
        
    }

    public void RequestAuthorizationTracking()
    {
#if UNITY_IOS
        Debug.Log("Unity iOS Support: Requesting iOS App Tracking Transparency native dialog.");
        Debug.Log("checking ATT Status" + ATTrackingStatusBinding.GetAuthorizationTrackingStatus());
        if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED) {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
            Debug.Log("checking ATT" + ATTrackingStatusBinding.GetAuthorizationTrackingStatus());
        }
        
#else
            Debug.LogWarning("Unity iOS Support: Tried to request iOS App Tracking Transparency native dialog, " +
                             "but the current platform is not iOS.");
#endif
    }
}