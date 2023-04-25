using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;
using static Oracle;

public class LevelCompleteManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text levelText;


    private Statistics stats => oracle.saveData.level.levelStats;
    private Level level => oracle.saveData.level;

    private void Update()
    {
        levelText.text = $"Stage: {(int)oracle.saveData.levelSelector + 1}";
        text.text =
            $"Time spent in Stage: {CalcUtils.FormatTimeLarge(stats.timeSpentInLevel)} <br>Skrimp captured: {stats.timesSkrimpGoneThroughPortal:N0} <br>Skrimp wasted: {stats.timesSkrimpHitGround:N0} <br>Dev Skrimp: {level.devSkrimp}";
    }
}