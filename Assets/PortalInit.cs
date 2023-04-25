using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Oracle;

public class PortalInit : MonoBehaviour
{
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject cloud;
    [SerializeField] private GameObject avoPortal;
    [SerializeField] private GameObject catPortal;

    private void Start()
    {
        SelectSkin();
    }

    private void SelectSkin()
    {
        switch (oracle.saveData.preferences.skinSelection)
        {
            case SkinSelection.Random:
                SetSkin(1);
                break;
            case SkinSelection.Skrimp:
                SetSkin(1);
                break;
            case SkinSelection.Rain:
                SetSkin(2);
                break;
            case SkinSelection.BsGames:
                SetSkin(3);
                break;
            case SkinSelection.Avocado:
                SetSkin(4);
                break;
            case SkinSelection.Cat:
                SetSkin(5);
                break;
        }
    }

    private void SetSkin(int skin)
    {
        switch (skin)
        {
            case 1:
                portal.SetActive(true);
                break;
            case 2:
                cloud.SetActive(true);
                break;
            case 3:
                portal.SetActive(true);
                break;
            case 4:
                avoPortal.SetActive(true);
                break;
            case 5:
                catPortal.SetActive(true);
                break;
            default:
                goto case 1;
        }
    }
}