using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Oracle;

public class MenuTabRemember : TabRememberer
{
    private Preferences pref => oracle.saveData.preferences;

    [SerializeField] private GameObject[] objectsToDisable;

    public void ButtonPrep()
    {
        tab1.interactable = true;
        tab2.interactable = true;
        tab3.interactable = true;
        tab4.interactable = true;
        foreach (var obj in objectsToDisable) obj.SetActive(false);
    }

    private void Start()
    {
        tab1.onClick.AddListener(() => SaveToOracle(0));
        tab2.onClick.AddListener(() => SaveToOracle(1));
        tab3.onClick.AddListener(() => SaveToOracle(2));
        tab4.onClick.AddListener(() => SaveToOracle(3));


        switch (pref.menuTabs)
        {
            case Tab.Tab1:
                SetTab(Tab.Tab1);
                break;
            case Tab.Tab2:
                SetTab(Tab.Tab2);
                break;
            case Tab.Tab3:
                SetTab(Tab.Tab3);
                break;
            case Tab.Tab4:
                SetTab(Tab.Tab4);
                break;
            default:
                Debug.Log("NoTab");
                break;
        }
    }

    private void SaveToOracle(int i)
    {
        pref.menuTabs = (Tab)i;
    }
}