using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using static Oracle;

public class StatisticsManager : MonoBehaviour
{
    [SerializeField] private LevelStatsReferences lsr;

    private Statistics stats => oracle.saveData.statistics;
    private readonly string TextColorBlue = "<color=#A3FFFE>";

    private void OnEnable()
    {
        lsr.levelTimerText.text =
            $"Total playtime: {TextColorBlue}{CalcUtils.FormatTimeLarge(stats.timeSpentInLevel)}</color> <br>Level: {TextColorBlue}{oracle.saveData.player.level:N0}</color>";
        lsr.levelStatsText.text =
            $"Skrimp caught: {TextColorBlue}{stats.timesSkrimpGoneThroughPortal:N0}</color> <br>Skrimp wasted: {TextColorBlue}{stats.timesSkrimpHitGround:N0}</color>";
        lsr.skrimpCountText.text = $"<sprite=12>{TextColorBlue}{stats.devSkrimpCreated}</color>";
    }
}