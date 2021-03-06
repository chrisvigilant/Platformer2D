﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //Keep  track what Level we are currently in
    //Load and unload levels
    //Keep track of the game state
    //Generate other Persistent Systems

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    public GameObject[] SystemPrefabs;
    public Events.EventGameState OnGameStateChanged;

    private List<GameObject> _instancedSystemPrefabs;
    List<AsyncOperation> _loadOperations;
    GameState _currentGameState = GameState.PREGAME;

    private string _currentLevelName = string.Empty;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstatiateSystemPrefabs();

        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);

    }

    private void Update()
    {
        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void OnLoadOperationcomplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
        }

        Debug.Log("Load Complete.");
    }
    void OnUnloadOperationcomplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete.");
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            UnloadLevel(_currentLevelName);
        }
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;

            default:
                break;
        }

        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
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

    public void StartGame()
    {
        LoadLevel("FreeAsset2D");
    }

    public void TogglePause()
    {
        
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartLevel()
    {
        UpdateState(GameState.PREGAME);
    }

    public void QuitGame()
    {
        //implement features for quitting (save shits and stuff) Also exit to main menu instead of Ecit application
        Application.Quit();
    }
}
