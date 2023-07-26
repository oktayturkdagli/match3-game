using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Level[] allLevels;
    [SerializeField] private Level currentLevel;
    [SerializeField] private GridManager gridManager;
    public bool isLevelActive = false;
    private int currentLevelIndex;
    
    public static event Action OnLevelLoaded;
    public static event Action OnLevelSucceed;
    public static event Action OnLevelFailed;
    
    public Level CurrentLevelData
    {
        get => currentLevel;
        set => currentLevel = value;
    }
    
    private void OnEnable()
    {
        GoalPanel.OnAllGoalsEnded += LevelSucceed;
        GoalPanel.OnGoalsFailed += LevelFailed;
    }
    
    private void OnDisable()
    {
        GoalPanel.OnAllGoalsEnded -= LevelSucceed;
        GoalPanel.OnGoalsFailed -= LevelFailed;
    }
    
    private void Start()
    {
        LoadLevel();
    }
    
    private void LoadLevel()
    {
        currentLevelIndex = PlayerPrefs.GetInt("Level", 0) % allLevels.Length;
        currentLevel = allLevels[currentLevelIndex];
        gridManager.currentLevel = currentLevel;
        gridManager.LoadLevelDataToGridManager();
        gridManager.SpawnInitialBlocks();
        gridManager.SetGridCornerSize();
        isLevelActive = true;
        OnLevelLoaded?.Invoke();
    }

    private void LevelFailed()
    {
        if (isLevelActive)
        {
            isLevelActive = false;
            OnLevelFailed?.Invoke();
        }
    }

    private void LevelSucceed()
    {
        if (isLevelActive)
        {
            isLevelActive = false;
            currentLevelIndex += 1;
            PlayerPrefs.SetInt("Level", currentLevelIndex);
            OnLevelSucceed?.Invoke();
        }
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}