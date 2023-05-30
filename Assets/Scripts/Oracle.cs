using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Oracle : SerializedMonoBehaviour
{
    private readonly string fileName = "betaTest";
    private readonly string saveExtension = ".beta";
    [SerializeField] public BuildNumberChecker buildNumber;

    public bool Loaded;

    private void Start()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        BsNewsGet();
        Loaded = false;
        Load();
        Loaded = true;
        InvokeRepeating(nameof(Save), 60, 60);
    }


    private void OnApplicationQuit()
    {
        Save();
    }

#if !UNITY_EDITOR
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
#if UNITY_IOS
            Load();
#elif UNITY_ANDROID
            Load();
#endif
        }
        if (!focus)
        {
            Save();
        }
    }
#endif


    #region NewsTicker

    public BsGamesData bsGamesData;
    public bool gotNews;

    [ContextMenu("BsNewsGet")]
    public async void BsNewsGet()
    {
        var url = "https://www.blindsidedgames.com/newsTicker";

        using var www = UnityWebRequest.Get(url);

        www.SetRequestHeader("Content-Type", "application/jason");
        var operation = www.SendWebRequest();

        while (!operation.isDone) await Task.Yield();

        if (www.result == UnityWebRequest.Result.Success)
        {
            bsGamesData = new BsGamesData();

            var newsjson = www.downloadHandler.text;
            //Debug.Log(json);
            bsGamesData = JsonUtility.FromJson<BsGamesData>(newsjson);
            gotNews = true;
        }
        else
        {
            Debug.Log($"error {www.error}");
        }
    }

    [Serializable]
    public class BsGamesData
    {
        public string latestGameName;
        public string latestGameLink;
        public string latestGameAppStore;
        public string newsTicker;
        public string patreons;
        public string idleDysonSwarm;
    }

    #endregion

    #region Oracle

    public SaveData saveData;

    private string _json;

    #region SaveMethods

    [ContextMenu("WipeAllData")]
    public void WipeAllData()
    {
        var savePrefs = saveData.preferences;
        File.Delete(Application.persistentDataPath + "/" + fileName + saveExtension);
        Load();
        saveData.preferences = savePrefs;
        Save();
        SceneManager.LoadScene(0);
    }

    [ContextMenu("Save")]
    public void Save()
    {
        saveData.dateQuitString = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
        saveData.buildNumber = buildNumber;
        SaveState(Application.persistentDataPath + "/" + fileName + saveExtension);
    }

    public void SaveState(string filePath)
    {
        var bytes = SerializationUtility.SerializeValue(saveData, DataFormat.JSON);
        File.WriteAllBytes(filePath, bytes);
    }

    public void Load()
    {
        Loaded = false;
        saveData = new SaveData();

        if (File.Exists(Application.persistentDataPath + "/" + fileName + saveExtension))
        {
            LoadState(Application.persistentDataPath + "/" + fileName + saveExtension);
        }

        else
        {
            saveData.dateStarted = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            Loaded = true;
        }

        savedLevels = saveData.savedLevels;
    }

    public void LoadState(string filePath)
    {
        if (!File.Exists(filePath)) return;

        var bytes = File.ReadAllBytes(filePath);
        saveData = SerializationUtility.DeserializeValue<SaveData>(bytes, DataFormat.JSON);
        Loaded = true;
    }


    /*private void AwayForSeconds()
    {
        if (string.IsNullOrEmpty(oracle.saveSettings.dateQuitString)) return;
        var dateStarted = DateTime.Parse(oracle.saveSettings.dateQuitString, CultureInfo.InvariantCulture);
        var dateNow = DateTime.UtcNow;
        var timespan = dateNow - dateStarted;
        var seconds = (float)timespan.TotalSeconds;
        if (seconds < 0) seconds = 0;
        saveSettings.sdPrestige.doubleTime += seconds;
        AwayFor?.Invoke(seconds);
    }*/

    #endregion

    #endregion

    #region LevelData

    public Data data;
    [Space(10)] public Dictionary<LevelSelector, LevelConditions> levelConditions = new();
    public Dictionary<LevelSelector, Level> savedLevels = new();

    [Serializable]
    public class Data
    {
        public double xpExponent = 1.24;
        public double xpForFirstLevel = 100;
        [Space(10)] public double gravityBaseCost;
        public double gravityCostMulti;
        public float gravityIncreasePerLevel = 0.1f;
        [Space(10)] public double portalBaseCost;
        public double portalCostMulti;
        public float portalIncreasePerLevel = 0.1f;
        [Space(10)] public double valueBaseCost;
        public double valueCostMulti;
        public float valueIncreasePerLevel = 0.1f;
        [Space(10)] public double countBaseCost;
        public double countCostMulti;
        public int countIncreasePerLevel = 1;
    }

    #endregion

    #region SaveData

    [Serializable]
    public class SaveData
    {
        public BuildNumberChecker buildNumber;
        public string dateStarted;
        public string dateQuitString;
        public NumberTypes notation;

        public Statistics statistics = new();
        public Preferences preferences = new();

        public LevelSelector levelSelector = LevelSelector.Level1;
        public Player player = new();
        public Level level = new();

        public Dictionary<LevelSelector, Level> savedLevels = new();
    }

    [Serializable]
    public class BuildNumberChecker
    {
        public long buildNumber;
    }

    [Serializable]
    public class Player : PlayerLevel
    {
    }

    [Serializable]
    public class PlayerLevel
    {
        public long level = 1;
        public long pointsToSpend;
        public double experience;
    }

    [Serializable]
    public class Level : LevelUpgrades
    {
        public Statistics levelStats = new();
        public bool levelComplete;
        public double currency;
        public int devSkrimp;

        public float devSkrimpCooldown = 30;

        public float portalLocation;
        public bool portalLocked;
        public bool statsExpanded;
    }

    [Serializable]
    public class LevelUpgrades
    {
        public float gravity = 0.3f;
        public int gravityUpgrades;

        public float portalSize = 1.1f;
        public int portalSizeUpgrades;

        public double skrimpValue = 1;
        public int skrimpValueUpgrades;
        public float valueMultiFromBonusSkrimp = 1;

        public int skrimpCount = 1;
        public int skrimpCountUpgrades;
    }

    [Serializable]
    public class LevelConditions
    {
        //public int skrimpToWin = 10;
        public double scoreToAdvance = 1000;
    }

    [Serializable]
    public class Statistics
    {
        public double timeSpentInLevel;
        public double timesSkrimpHitGround;
        public double timesSkrimpGoneThroughPortal;
        public double devSkrimpCreated;
    }

    [Serializable]
    public class Preferences
    {
        public Tab menuTabs = Tab.Tab1;
        public SkinSelection skinSelection = SkinSelection.Skrimp;
        public CameraColor cameraColorPrefs = CameraColor.Blue;
        public SkrimpOnScreen skrimpOnScreen = SkrimpOnScreen.Fifty;
        public int skrimpOnScreenCount = 50;
        public bool rainSkinOwned;
        public bool avoSkinOwned;
        public bool catSkinOwned;
        public bool removeAds;
        public bool gyroEnabled;
        public bool loadLastLevelOnStart;
        public bool inGame;

        public bool fixedBug;
    }

    #endregion

    #region Enums

    public enum CameraColor
    {
        Blue,
        Black
    }

    public enum LevelSelector
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        Level10,
        Level11,
        Level12,
        Level13,
        Level14,
        Level15,
        Level16,
        Level17,
        Level18,
        Level19,
        Level20,
        Level21,
        Level22,
        Level23,
        Level24,
        Level25,
        Level26,
        Level27,
        Level28,
        Level29,
        Level30
    }

    public enum SkrimpOnScreen
    {
        Fifty,
        OneHundred,
        Unlimited,
        MegaSkrimp
    }

    public enum SkinSelection
    {
        Random,
        Skrimp,
        Rain,
        BsGames,
        Avocado,
        Cat
    }

    public enum Tab
    {
        Tab1,
        Tab2,
        Tab3,
        Tab4
    }

    public enum BuyMode
    {
        Buy1,
        Buy10,
        Buy50,
        Buy100,
        BuyMax
    }

    public enum ResearchBuyMode
    {
        Buy1,
        Buy10,
        Buy50,
        Buy100,
        BuyMax
    }

    public enum NumberTypes
    {
        Standard,
        Scientific,
        Engineering
    }

    #endregion


    #region Singleton class: Oracle

    public static Oracle oracle;


    private void Awake()
    {
        if (oracle == null)
            oracle = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    #endregion

    #region CloudSaving/loading

    [ContextMenu("SaveTOCloud")]
    public void SaveToCloud()
    {
        SaveSomeData("test");
    }

    [ContextMenu("LoadFromCloud")]
    public void LoadFromCloud()
    {
        LoadSomeData("test");
    }

    public event Action<string> SaveDataSuccessful;
    public event Action<string> SaveLoadError;
    public event Action<string> LoadDataSuccessful;

    public async void SaveSomeData(string saveName)
    {
        saveData.dateQuitString = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
        var dataString = Encoding.UTF8.GetString(SerializationUtility.SerializeValue(saveData, DataFormat.JSON));
        var data = new Dictionary<string, object> { { saveName, dataString } };
        try
        {
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.HResult);
            SaveLoadError?.Invoke(e.ToString());
            throw;
        }

        SaveDataSuccessful?.Invoke(saveName);
        FindObjectOfType<SettingsManager>().RetrieveKeys();
    }

    public async void LoadSomeData(string saveName)
    {
        try
        {
            var savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { saveName });
            Loaded = false;
            var bytes = Encoding.UTF8.GetBytes(savedData[saveName]);
            saveData = SerializationUtility.DeserializeValue<SaveData>(bytes, DataFormat.JSON);
            Loaded = true;
            StartCoroutine(ReloadScene(saveName));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetBaseException());
            SaveLoadError?.Invoke(e.GetBaseException().Message);
            throw;
        }
    }

    private IEnumerator ReloadScene(string saveName)
    {
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(0.1f);
        LoadDataSuccessful?.Invoke(saveName);
    }

    #endregion
}