using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkrimpInterface;
using static Oracle;


public class SkrimpManager : MonoBehaviour
{
    private Level level => skrimpInterface.level;
    public bool devSkrimp;
    public static int skrimCount;
    public static int devSkrimpCount;

    [SerializeField] private GameObject skrimpSkin;
    [SerializeField] private GameObject rainSkin;
    [SerializeField] private GameObject bsGamesSkins;
    [SerializeField] private GameObject avoSkin;
    [SerializeField] private GameObject catSkin;


    private void Start()
    {
        UpdateSkrimpStats();
        if (devSkrimp)
        {
            devSkrimpCount++;
        }
        else
        {
            skrimCount++;
            SelectSkin();
        }
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
                skrimpSkin.SetActive(true);
                break;
            case 2:
                rainSkin.SetActive(true);
                break;
            case 3:
                bsGamesSkins.SetActive(true);
                break;
            case 4:
                avoSkin.SetActive(true);
                break;
            case 5:
                catSkin.SetActive(true);
                break;
            default:
                goto case 1;
        }
    }

    private void OnEnable()
    {
        skrimpInterface.UpdateSkrimps += UpdateSkrimpStats;
    }

    private void OnDisable()
    {
        skrimpInterface.UpdateSkrimps -= UpdateSkrimpStats;
    }

    private void UpdateSkrimpStats()
    {
        GetComponent<Rigidbody2D>().gravityScale = level.gravity;
    }
}