using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Oracle;
using CloudOnce;

public class LeaderboardSubmitter : LevelSystem
{
    private Player player => oracle.saveData.player;

    private void Start()
    {
        Leaderboards.playerLevel.SubmitScore((long)((player.level + XpToLevel(player.experience, player.level)) * 100));
    }
}