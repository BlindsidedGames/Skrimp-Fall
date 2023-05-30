using System;
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
    [SerializeField] private Toggle loadSceneOnStart;

    [SerializeField] private TMP_Dropdown skrimpCountDropdown;
    [SerializeField] private Slider skrimpCountSlider;
    [SerializeField] private TMP_Text skrimpCountText;

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
            oracle.saveData.preferences.gyroEnabled = arg0);
        loadSceneOnStart.onValueChanged.AddListener(arg0 =>
            oracle.saveData.preferences.loadLastLevelOnStart = arg0);

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
        loadSceneOnStart.SetIsOnWithoutNotify(oracle.saveData.preferences.loadLastLevelOnStart);


        skrimpCountDropdown.value = (int)oracle.saveData.preferences.skrimpOnScreen;
        skrimpCountDropdown.onValueChanged.AddListener(i =>
        {
            oracle.saveData.preferences.skrimpOnScreen = (SkrimpOnScreen)i;
            SetSliderValues();
        });
        SetSliderValues();
        skrimpCountSlider.onValueChanged.AddListener(i =>
        {
            oracle.saveData.preferences.skrimpOnScreenCount = (int)i;
            SetSkrimpCountText();
        });
        SetSkrimpCountText();
        skrimpCountSlider.value = oracle.saveData.preferences.skrimpOnScreenCount;
        skrimpCountDropdown.value = (int)oracle.saveData.preferences.skrimpOnScreen;
    }

    private void SetSliderValues()
    {
        var numberToSpawn = 50;
        skrimpCountSlider.interactable = true;
        switch (oracle.saveData.preferences.skrimpOnScreen)
        {
            case SkrimpOnScreen.Fifty:
                numberToSpawn = 50;
                break;
            case SkrimpOnScreen.OneHundred:
                numberToSpawn = 100;
                break;
            case SkrimpOnScreen.Unlimited:
                numberToSpawn = 2147483647;
                skrimpCountSlider.interactable = false;
                break;
            case SkrimpOnScreen.MegaSkrimp:
                numberToSpawn = 1;
                skrimpCountSlider.interactable = false;
                break;
            default:
                goto case SkrimpOnScreen.Fifty;
        }

        skrimpCountSlider.maxValue = numberToSpawn;
        skrimpCountSlider.value = numberToSpawn;
    }

    private void SetSkrimpCountText()
    {
        skrimpCountText.text =
            $"Max Skrimp on screen: {(oracle.saveData.preferences.skrimpOnScreen == SkrimpOnScreen.Unlimited ? "Unlimited" : oracle.saveData.preferences.skrimpOnScreenCount)}";
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