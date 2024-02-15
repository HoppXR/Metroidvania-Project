using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private FileDataHandler dataHandler;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    public static DataPersistenceManager instance { get; private set; }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data was found, initializing data to defaults");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    // Method to handle manual save trigger
    public void ManualSave()
    {
        SaveGame();
        Debug.Log("Game saved manually.");
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
