using UnityEngine;

[CreateAssetMenu(fileName = "WeightedSpawnConfig", menuName = "SO/Weight")]
public class WeightedSpawnSO : ScriptableObject
{
    public int obj;

    [Range(0, 1)] public double MinWeight;

    [Range(0, 1)] public double MaxWeight;

    public float GetWeight()
    {
        return Random.Range((float)MinWeight, (float)MaxWeight);
    }
}