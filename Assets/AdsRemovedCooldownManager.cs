using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Oracle;
using static SkrimpInterface;
using CloudOnce;
using Utilities;

public class AdsRemovedCooldownManager : MonoBehaviour
{
    [SerializeField] private Image cooldownFill;
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private Button getSkrimpButton;
    [SerializeField] private GameObject cooldownOverlay;

    private Preferences prefs => oracle.saveData.preferences;
    private readonly float cooldownTime = 30;

    private void Start()
    {
        getSkrimpButton.onClick.AddListener(GetSkrimp);
    }


    private void Update()
    {
        var interactable = skrimpInterface.level.devSkrimpCooldown >= cooldownTime;
        getSkrimpButton.interactable = interactable;
        cooldownOverlay.SetActive(!interactable);

        if (skrimpInterface.level.devSkrimpCooldown < cooldownTime)
            skrimpInterface.level.devSkrimpCooldown += Time.deltaTime;
        cooldownFill.fillAmount = (cooldownTime - skrimpInterface.level.devSkrimpCooldown) / cooldownTime;
        cooldownText.text = $"{cooldownTime - skrimpInterface.level.devSkrimpCooldown:N0}";
    }

    private void GetSkrimp()
    {
        skrimpInterface.level.devSkrimp++;
        oracle.saveData.statistics.devSkrimpCreated++;
        oracle.saveData.level.levelStats.devSkrimpCreated++;
        Leaderboards.devSkrimpPlus.SubmitScore((int)oracle.saveData.statistics.devSkrimpCreated);
        skrimpInterface.UpdateSkrimp();
        skrimpInterface.level.devSkrimpCooldown = 0;
    }
}