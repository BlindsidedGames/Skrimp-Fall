using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Oracle;

public class ToggleObjectsInLevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToToggle;
    public LevelSelector level;
    private SaveData sd => oracle.saveData;

    public void ToggleAllObjects()
    {
        foreach (var obj in objectsToToggle) obj.SetActive(!obj.activeSelf);
        sd.savedLevels[level].statsExpanded = !sd.savedLevels[level].statsExpanded;
    }

    public void ToggleStatsForLevelLoader()
    {
        foreach (var obj in objectsToToggle) obj.SetActive(!obj.activeSelf);
    }
}