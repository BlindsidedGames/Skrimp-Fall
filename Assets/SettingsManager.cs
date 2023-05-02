using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.UI;
using static Oracle;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Button save1;
    [SerializeField] private Button save2;
    [SerializeField] private Button load1;
    [SerializeField] private Button load2;

    [SerializeField] private Button setBlue;
    [SerializeField] private Button setBlack;

    [SerializeField] private Button wipeSave;

    [SerializeField] private Button accountSettings;
    [SerializeField] private GameObject accountSettingsGO;
    [SerializeField] private Toggle gyroControlSetting;

    [SerializeField] private TMP_Dropdown skrimpCountDropdown;

    private void Start()
    {
        save1.onClick.AddListener(() => oracle.SaveSomeData("Slot_1"));
        save2.onClick.AddListener(() => oracle.SaveSomeData("Slot_2"));

        load1.onClick.AddListener(() => oracle.LoadSomeData("Slot_1"));

        load2.onClick.AddListener(() => oracle.LoadSomeData("Slot_2"));
        wipeSave.onClick.AddListener(() => oracle.WipeAllData());
        accountSettings.onClick.AddListener(() => accountSettingsGO.SetActive(!accountSettingsGO.activeSelf));

        setBlack.onClick.AddListener(() => oracle.saveData.preferences.cameraColorPrefs = CameraColor.Black);
        setBlue.onClick.AddListener(() => oracle.saveData.preferences.cameraColorPrefs = CameraColor.Blue);

        gyroControlSetting.onValueChanged.AddListener(arg0 =>
            oracle.saveData.preferences.gyroEnabled = gyroControlSetting.isOn);

        switch (oracle.saveData.preferences.cameraColorPrefs)
        {
            case CameraColor.Blue:
                setBlack.gameObject.SetActive(true);
                break;
            case CameraColor.Black:
                setBlue.gameObject.SetActive(true);
                break;
        }

        gyroControlSetting.SetIsOnWithoutNotify(oracle.saveData.preferences.gyroEnabled);
        skrimpCountDropdown.value = (int)oracle.saveData.preferences.skrimpOnScreen;
        skrimpCountDropdown.onValueChanged.AddListener(i =>
            oracle.saveData.preferences.skrimpOnScreen = (SkrimpOnScreen)i);
    }


    public async void RetrieveKeys()
    {
        try
        {
            var keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();
            load1.interactable = false;
            load2.interactable = false;
            for (var i = 0; i < keys.Count; i++)
            {
                Debug.Log(keys[i]);
                if (keys[i] == "Slot_1") load1.interactable = true;
                if (keys[i] == "Slot_2") load2.interactable = true;
            }
        }
        catch (Exception e)
        {
            load1.interactable = false;
            load2.interactable = false;
            throw;
        }
    }
}