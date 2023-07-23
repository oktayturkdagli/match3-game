using System;
using UnityEngine;

public class GoalPanel : Singleton<GoalPanel>
{
    [SerializeField] private GoalObject blueCubeGoal;
    [SerializeField] private GoalObject greenCubeGoal;
    [SerializeField] private GoalObject pinkCubeGoal;
    [SerializeField] private GoalObject purpleCubeGoal;
    [SerializeField] private GoalObject redCubeGoal;
    [SerializeField] private GoalObject yellowCubeGoal;
    
    private Goals currentGoals;
    
    public static event Action allGoalsEndedEvent;
    public static event Action goalsFailedEvent;
    
    private void OnEnable()
    {
        LevelManager.levelLoadedEvent += SetupGoalObjects;
        MovesPanel.movesFinishedEvent += CheckDidGoalsFail;
    }
    
    private void OnDisable()
    {
        LevelManager.levelLoadedEvent -= SetupGoalObjects;
        MovesPanel.movesFinishedEvent -= CheckDidGoalsFail;
    }
    
    private void SetupGoalObjects()
    {
        currentGoals = LevelManager.Instance.CurrentLevelData.goals;
        
        blueCubeGoal.Count = currentGoals.blueCubeCount;
        greenCubeGoal.Count = currentGoals.greenCubeCount;
        pinkCubeGoal.Count = currentGoals.pinkCubeCount;
        purpleCubeGoal.Count = currentGoals.purpleCubeCount;
        redCubeGoal.Count = currentGoals.redCubeCount;
        yellowCubeGoal.Count = currentGoals.yellowCubeCount;
        
        if (blueCubeGoal.Count > 0)
            blueCubeGoal.gameObject.SetActive(true);
        if (greenCubeGoal.Count > 0)
            greenCubeGoal.gameObject.SetActive(true);
        if (pinkCubeGoal.Count > 0)
            pinkCubeGoal.gameObject.SetActive(true);
        if (purpleCubeGoal.Count > 0)
            purpleCubeGoal.gameObject.SetActive(true);
        if (redCubeGoal.Count > 0)
            redCubeGoal.gameObject.SetActive(true);
        if (yellowCubeGoal.Count > 0)
            yellowCubeGoal.gameObject.SetActive(true);
    }
    
    public void DecreaseGoalFromCubeType(CubeTypes cubeType)
    {
        GoalObject goalObject = FindGoalFromCubeType(cubeType);
        
        if (goalObject)
            goalObject.Count -= 1;
        
        if (CheckDidGoalsAchieve())
            allGoalsEndedEvent?.Invoke();
    }
    
    private bool CheckDidGoalsAchieve()
    {
        bool areAllGoalsEnded = true;
        
        if (blueCubeGoal.gameObject.activeSelf && blueCubeGoal.Count > 0)
            areAllGoalsEnded = false;
        if (greenCubeGoal.gameObject.activeSelf && greenCubeGoal.Count > 0)
            areAllGoalsEnded = false;
        if (pinkCubeGoal.gameObject.activeSelf && pinkCubeGoal.Count > 0)
            areAllGoalsEnded = false;
        if (purpleCubeGoal.gameObject.activeSelf && purpleCubeGoal.Count > 0)
            areAllGoalsEnded = false;
        if (redCubeGoal.gameObject.activeSelf && redCubeGoal.Count > 0)
            areAllGoalsEnded = false;
        if (yellowCubeGoal.gameObject.activeSelf && yellowCubeGoal.Count > 0)
            areAllGoalsEnded = false;
        
        return areAllGoalsEnded;
    }
    
    private void CheckDidGoalsFail()
    {
        if (!CheckDidGoalsAchieve())
            goalsFailedEvent?.Invoke();
    }
    
    private GoalObject FindGoalFromCubeType(CubeTypes cubeType)
    {
        switch (cubeType)
        {
            case CubeTypes.Blue:
                return blueCubeGoal;
            case CubeTypes.Green:
                return greenCubeGoal;
            case CubeTypes.Pink:
                return pinkCubeGoal;
            case CubeTypes.Purple:
                return purpleCubeGoal;
            case CubeTypes.Red:
                return redCubeGoal;
            case CubeTypes.Yellow:
                return yellowCubeGoal;
            default: 
                return blueCubeGoal;
        }
    }
    
    public Vector3 GetGoalPositionFromCubeType(CubeTypes cubeType)
    {
        switch (cubeType)
        {
            case CubeTypes.Blue:
                return blueCubeGoal.gameObject.transform.position;
            case CubeTypes.Green:
                return greenCubeGoal.gameObject.transform.position;
            case CubeTypes.Pink:
                return pinkCubeGoal.gameObject.transform.position;
            case CubeTypes.Purple:
                return purpleCubeGoal.gameObject.transform.position;
            case CubeTypes.Red:
                return redCubeGoal.gameObject.transform.position;
            case CubeTypes.Yellow:
                return yellowCubeGoal.gameObject.transform.position;
            default: 
                return redCubeGoal.gameObject.transform.position;
        }
    }
    
    public bool CheckIsThereGoalFromCubeType(CubeTypes cubeType)
    {
        switch (cubeType)
        {
            case CubeTypes.Blue:
                return blueCubeGoal.Count > 0;
            case CubeTypes.Green:
                return greenCubeGoal.Count > 0;
            case CubeTypes.Pink:
                return pinkCubeGoal.Count > 0;
            case CubeTypes.Purple:
                return purpleCubeGoal.Count > 0;
            case CubeTypes.Red:
                return redCubeGoal.Count > 0;
            case CubeTypes.Yellow:
                return yellowCubeGoal.Count > 0;
            
            default:
                return false;
        }
    }
}