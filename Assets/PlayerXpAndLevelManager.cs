using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Oracle;

public class PlayerXpAndLevelManager : LevelSystem
{
    [SerializeField] private TMP_Text playerLevelText;
    [SerializeField] private SlicedFilledImage xpBar;
    [SerializeField] private SkrimpInterface _skrimpInterface;

    private Player player => oracle.saveData.player;

    private void Update()
    {
        xpBar.fillAmount = (float)XpToLevel(player.experience, player.level);
        playerLevelText.text = $"Lvl {player.level}";

        if (Leveled(player.level, player.experience))
        {
            player.experience -= LevelCost(player.level);
            player.pointsToSpend++;
            player.level++;
            _skrimpInterface.UpdateSkrimp();
        }
    }
}