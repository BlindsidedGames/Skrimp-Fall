using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Oracle;

public class LoadOnGameStartManager : MonoBehaviour
{
    private Preferences prefs => oracle.saveData.preferences;

    [SerializeField] private LevelLoader lvlLoader;

    // Start is called before the first frame update
    private void Start()
    {
        if (prefs.loadLastLevelOnStart && prefs.inGame) lvlLoader.LoadScene();
    }
}