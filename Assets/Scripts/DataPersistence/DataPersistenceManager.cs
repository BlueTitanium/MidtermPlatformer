using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]

    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance {get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void Start() {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        if(scene.name != "MainMenu" || scene.name != "EndScene")
        {
            LoadGame();
        }

        print("loading scene..." + this.gameData.currWeaponLength);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;        
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void NewGame()
    {
        Debug.Log("Creating new game");
        this.gameData = new GameData();
        print("initial checkpoint" + this.gameData.checkPoint);
        SaveGame();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach (IDataPersistence dataPersistencObj in dataPersistenceObjects)
        {
            dataPersistencObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistencObj in dataPersistenceObjects)
        {
            dataPersistencObj.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    public string getScene()
    {
        return gameData.Level;
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
