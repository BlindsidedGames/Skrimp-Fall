using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;
using static Oracle;

public class SaveAndLoadListener : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void OnEnable()
    {
        oracle.SaveDataSuccessful += Saved;
        oracle.SaveLoadError += Error;
        oracle.LoadDataSuccessful += Loaded;
    }

    private void OnDisable()
    {
        oracle.SaveDataSuccessful -= Saved;
        oracle.SaveLoadError -= Error;
        oracle.LoadDataSuccessful -= Loaded;
    }

    private void Saved(string slotName)
    {
        var name = "";
        switch (slotName)
        {
            case "Slot_1":
                name = "Slot 1";
                break;
            case "Slot_2":
                name = "Slot 2";
                break;
            default:
                name = slotName;
                break;
        }

        text.text = $"Success! Saved to: {name}";
    }

    private void Error(string error)
    {
        text.text = $"Error: {error}";
    }

    private void Loaded(string slotName)
    {
        var name = "";
        switch (slotName)
        {
            case "Slot_1":
                name = "Slot 1";
                break;
            case "Slot_2":
                name = "Slot 2";
                break;
            default:
                name = slotName;
                break;
        }

        text.text = $"Success! Loaded from: {name}";
    }
}