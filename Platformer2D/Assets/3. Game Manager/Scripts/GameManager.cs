using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //Keep  track what Level we are currently in
    //Load and unload levels
    //Keep track of the game state
    //Generate other Persistent Systems

    public GameObject[] SystemPrefabs;

    private List<GameObject> _instancedSystemPrefabs;
    List<AsyncOperation> _loadOperations;

    private string _currentLevelName = string.Empty;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstatiateSystemPrefabs();

        LoadLevel("FirstLevel");
    }

    void OnLoadOperationcomplete(AsyncOperation ao)
    {
        Debug.Log("Load Complete.");
        _loadOperations.Remove(ao);
    }
    void OnUnloadOperationcomplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete.");
    }

    void InstatiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i =0; i <SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);

        }
    }
    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("Unable to Load Level: " + levelName);
            return;
        }
        ao.completed += OnLoadOperationcomplete;
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
    }
    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("Unable to Unload Level: " + levelName);
            return;
        }
        ao.completed += OnUnloadOperationcomplete;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i =0; i < _instancedSystemPrefabs.Count; ++i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }
}
