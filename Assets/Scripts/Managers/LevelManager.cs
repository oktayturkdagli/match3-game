using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Level[] allLevels;
    [SerializeField] private Level currentLevelData;
    [SerializeField] private GridManager gridManager;
    public bool isLevelActive = false;
    private int currentLevelIndex;
    
    public static event Action levelLoadedEvent;
    public static event Action levelSuccesedEvent;
    public static event Action levelFailedEvent;
    
    public Level CurrentLevelData
    {
        get => currentLevelData;
        set => currentLevelData = value;
    }
    
    private void OnEnable()
    {
        GoalPanel.allGoalsEndedEvent += LevelSucceed;
        GoalPanel.goalsFailedEvent += LevelFailed;
    }
    
    private void OnDisable()
    {
        GoalPanel.allGoalsEndedEvent -= LevelSucceed;
        GoalPanel.goalsFailedEvent -= LevelFailed;
    }
    
    private void Start()
    {
        LoadLevel();
    }
    
    private void LoadLevel()
    {
        currentLevelIndex = PlayerPrefs.GetInt("Level", 0) % allLevels.Length;
        currentLevelData = allLevels[currentLevelIndex];
        gridManager.currentLevelData = currentLevelData;
        gridManager.LoadLevelDataToGridManager();
        gridManager.SpawnStartingBlocks();
        gridManager.SetGridCornerSize();
        isLevelActive = true;
        levelLoadedEvent?.Invoke();
    }
    
    public void LevelFailed()
    {
        if (isLevelActive)
        {
            isLevelActive = false;
            levelFailedEvent?.Invoke();
        }
    }
    
    public void LevelSucceed()
    {
        if (isLevelActive)
        {
            isLevelActive = false;
            currentLevelIndex += 1;
            PlayerPrefs.SetInt("Level", currentLevelIndex);
            levelSuccesedEvent?.Invoke();
        }
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}