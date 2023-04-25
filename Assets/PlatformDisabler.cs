using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDisabler : MonoBehaviour
{
    public bool disableOnAndroid;
    public bool disableOniOS;

    public GameObject[] objectsToDisable;


    private void Awake()
    {
#if UNITY_IOS
    if (disableOniOS)
        {
            foreach (var obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
        }
#endif

#if UNITY_ANDROID
        if (disableOnAndroid)
            foreach (var obj in objectsToDisable)
                obj.SetActive(false);
#endif
    }
}