using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using static Oracle;

public class LevelStatsInit : MonoBehaviour
{
    [SerializeField] private LevelLoader lvlLoader;
    [SerializeField] private Transform levelSelectScrollViewContent;

    [SerializeField] private Transform levelStatsPrefab;
    private SaveData sd => oracle.saveData;
    private readonly string TextColorBlue = "<color=#A3FFFE>";

    //private Level lvl => oracle.saveData.level;

    private void Start()
    {
        if (sd.savedLevels == null)
        {
            sd.savedLevels = new Dictionary<LevelSelector, Level>();
            sd.levelSelector = LevelSelector.Level1;
            sd.savedLevels.Add(LevelSelector.Level1, new Level());
        }

        if (sd.savedLevels.Count < 1)
        {
            sd.levelSelector = LevelSelector.Level1;
            sd.savedLevels.Add(LevelSelector.Level1, new Level());
        }

        SetLevelSelect();
    }

    private void SetLevelSelect()
    {
        foreach (var level in sd.savedLevels)
        {
            var lsr = Instantiate(levelStatsPrefab, levelSelectScrollViewContent)
                .GetComponent<LevelStatsReferences>();
            lsr.levelNameText.text = $"Stage {(int)level.Key + 1}";
            lsr.levelTimerText.text =
                $"Time spent in level: {TextColorBlue}{CalcUtils.FormatTimeLarge(level.Value.levelStats.timeSpentInLevel)}</color>  ";
            lsr.levelStatsText.text =
                $"Value: {TextColorBlue}{CalcUtils.FormatNumber(level.Value.currency)}</color><br>Skrimp Captured: {TextColorBlue}{level.Value.levelStats.timesSkrimpGoneThroughPortal:N0}</color> <br>Skrimp Wasted: {TextColorBlue}{level.Value.levelStats.timesSkrimpHitGround:N0}</color>";
            lsr.skrimpCountText.text =
                $"<sprite=14> {TextColorBlue}{level.Value.skrimpCount}</color><br><sprite=12> {TextColorBlue}{level.Value.devSkrimp}</color>";
            lsr.loadLevelButton.onClick.AddListener(() => SetLevelToLoad(level.Key));
            lsr.resetLevelButton.onClick.AddListener(() => ResetLevel(level.Key));
            lsr.toggleObjectsInLevelLoader.level = level.Key;
            if (level.Value.statsExpanded) lsr.toggleObjectsInLevelLoader.ToggleStatsForLevelLoader();
        }
    }

    private void ResetLevel(LevelSelector level)
    {
        sd.savedLevels[level] = new Level();
        SceneManager.LoadScene(0);
    }

    private void SetLevelToLoad(LevelSelector level)
    {
        sd.levelSelector = level;
        sd.level = sd.savedLevels[level];
        lvlLoader.LoadScene();
    }
}