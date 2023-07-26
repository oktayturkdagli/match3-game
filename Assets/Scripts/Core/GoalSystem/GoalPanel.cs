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
    
    public static event Action OnAllGoalsEnded;
    public static event Action OnGoalsFailed;
    
    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += Initialize;
        MovesPanel.OnMovesFinished += CheckDidGoalsFail;
    }
    
    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= Initialize;
        MovesPanel.OnMovesFinished -= CheckDidGoalsFail;
    }
    
    private void Initialize()
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
    
    public void DecreaseGoal(BlockTypes blockType)
    {
        GoalObject goalObject = GetGoalObject(blockType);
        
        if (goalObject)
            goalObject.Count -= 1;
        
        if (CheckDidGoalsAchieve())
            OnAllGoalsEnded?.Invoke();
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
            OnGoalsFailed?.Invoke();
    }
    
    public bool CheckIsThereGoal(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Blue:
                return blueCubeGoal.Count > 0;
            case BlockTypes.Green:
                return greenCubeGoal.Count > 0;
            case BlockTypes.Pink:
                return pinkCubeGoal.Count > 0;
            case BlockTypes.Purple:
                return purpleCubeGoal.Count > 0;
            case BlockTypes.Red:
                return redCubeGoal.Count > 0;
            case BlockTypes.Yellow:
                return yellowCubeGoal.Count > 0;
            
            default:
                return false;
        }
    }
    
    private GoalObject GetGoalObject(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Blue:
                return blueCubeGoal;
            case BlockTypes.Green:
                return greenCubeGoal;
            case BlockTypes.Pink:
                return pinkCubeGoal;
            case BlockTypes.Purple:
                return purpleCubeGoal;
            case BlockTypes.Red:
                return redCubeGoal;
            case BlockTypes.Yellow:
                return yellowCubeGoal;
            default: 
                return blueCubeGoal;
        }
    }
    
    public Vector3 GetGoalPosition(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Blue:
                return blueCubeGoal.gameObject.transform.position;
            case BlockTypes.Green:
                return greenCubeGoal.gameObject.transform.position;
            case BlockTypes.Pink:
                return pinkCubeGoal.gameObject.transform.position;
            case BlockTypes.Purple:
                return purpleCubeGoal.gameObject.transform.position;
            case BlockTypes.Red:
                return redCubeGoal.gameObject.transform.position;
            case BlockTypes.Yellow:
                return yellowCubeGoal.gameObject.transform.position;
            default: 
                return redCubeGoal.gameObject.transform.position;
        }
    }
}