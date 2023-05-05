using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Oracle;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Button btn;
    private Preferences prefs => oracle.saveData.preferences;

    private void Start()
    {
        if (btn != null) btn.onClick.AddListener(LoadScene);
    }

    public void LoadScene()
    {
        var level = "";
        switch (oracle.saveData.levelSelector)
        {
            case LevelSelector.Level1:
                level = "Level1";
                break;
            case LevelSelector.Level2:
                level = "Level2";
                break;
            case LevelSelector.Level3:
                level = "Level3";
                break;
            case LevelSelector.Level4:
                level = "Level4";
                break;
            case LevelSelector.Level5:
                level = "Level5";
                break;
            case LevelSelector.Level6:
                level = "Level6";
                break;
            case LevelSelector.Level7:
                level = "Level7";
                break;
            case LevelSelector.Level8:
                level = "Level8";
                break;
            case LevelSelector.Level9:
                level = "Level9";
                break;
            case LevelSelector.Level10:
                level = "Level10";
                break;
            case LevelSelector.Level11:
                level = "Level11";
                break;
            case LevelSelector.Level12:
                level = "Level12";
                break;
            case LevelSelector.Level13:
                level = "Level13";
                break;
            case LevelSelector.Level14:
                level = "Level14";
                break;
            case LevelSelector.Level15:
                level = "Level15";
                break;
            case LevelSelector.Level16:
                level = "Level16";
                break;
            case LevelSelector.Level17:
                level = "Level17";
                break;
            case LevelSelector.Level18:
                level = "Level18";
                break;
            case LevelSelector.Level19:
                level = "Level19";
                break;
            case LevelSelector.Level20:
                level = "Level20";
                break;
            case LevelSelector.Level21:
                level = "Level21";
                break;
            case LevelSelector.Level22:
                level = "Level22";
                break;
            case LevelSelector.Level23:
                level = "Level23";
                break;
            case LevelSelector.Level24:
                level = "Level24";
                break;
            default:
                oracle.saveData.levelSelector = LevelSelector.Level1;
                goto case LevelSelector.Level1;
        }

        prefs.inGame = true;
        oracle.saveData.level = oracle.saveData.savedLevels[oracle.saveData.levelSelector];
        SkrimpManager.skrimCount = 0;
        SkrimpManager.devSkrimpCount = 0;
        SceneManager.LoadScene(level);
    }
}