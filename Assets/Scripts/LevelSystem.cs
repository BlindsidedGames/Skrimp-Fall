using UnityEngine;
using static Oracle;
using Utilities;

public class LevelSystem : MonoBehaviour
{
    protected bool Leveled(long level, double xp)
    {
        return xp >= LevelCost(level);
    }

    protected double LevelCost(long level)
    {
        return CalcUtils.BuyX(1, oracle.data.xpForFirstLevel, oracle.data.xpExponent, level);
    }

    protected double XpToLevel(double currentXp, long currentLevel)
    {
        return currentXp / LevelCost(currentLevel);
    }
}