using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GoalPanel : Singleton<GoalPanel>
{
    [SerializeField] private GoalObject blueBlockGoal;
    [SerializeField] private GoalObject greenBlockGoal;
    [SerializeField] private GoalObject pinkBlockGoal;
    [SerializeField] private GoalObject purpleBlockGoal;
    [SerializeField] private GoalObject redBlockGoal;
    [SerializeField] private GoalObject yellowBlockGoal;
    
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
        
        blueBlockGoal.Count = currentGoals.blueBlockCount;
        greenBlockGoal.Count = currentGoals.greenBlockCount;
        pinkBlockGoal.Count = currentGoals.pinkBlockCount;
        purpleBlockGoal.Count = currentGoals.purpleBlockCount;
        redBlockGoal.Count = currentGoals.redBlockCount;
        yellowBlockGoal.Count = currentGoals.yellowBlockCount;
        
        if (blueBlockGoal.Count > 0)
            blueBlockGoal.gameObject.SetActive(true);
        if (greenBlockGoal.Count > 0)
            greenBlockGoal.gameObject.SetActive(true);
        if (pinkBlockGoal.Count > 0)
            pinkBlockGoal.gameObject.SetActive(true);
        if (purpleBlockGoal.Count > 0)
            purpleBlockGoal.gameObject.SetActive(true);
        if (redBlockGoal.Count > 0)
            redBlockGoal.gameObject.SetActive(true);
        if (yellowBlockGoal.Count > 0)
            yellowBlockGoal.gameObject.SetActive(true);
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
        
        if (blueBlockGoal.gameObject.activeSelf && blueBlockGoal.Count > 0)
            areAllGoalsEnded = false;
        if (greenBlockGoal.gameObject.activeSelf && greenBlockGoal.Count > 0)
            areAllGoalsEnded = false;
        if (pinkBlockGoal.gameObject.activeSelf && pinkBlockGoal.Count > 0)
            areAllGoalsEnded = false;
        if (purpleBlockGoal.gameObject.activeSelf && purpleBlockGoal.Count > 0)
            areAllGoalsEnded = false;
        if (redBlockGoal.gameObject.activeSelf && redBlockGoal.Count > 0)
            areAllGoalsEnded = false;
        if (yellowBlockGoal.gameObject.activeSelf && yellowBlockGoal.Count > 0)
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
                return blueBlockGoal.Count > 0;
            case BlockTypes.Green:
                return greenBlockGoal.Count > 0;
            case BlockTypes.Pink:
                return pinkBlockGoal.Count > 0;
            case BlockTypes.Purple:
                return purpleBlockGoal.Count > 0;
            case BlockTypes.Red:
                return redBlockGoal.Count > 0;
            case BlockTypes.Yellow:
                return yellowBlockGoal.Count > 0;
            
            default:
                return false;
        }
    }
    
    private GoalObject GetGoalObject(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Blue:
                return blueBlockGoal;
            case BlockTypes.Green:
                return greenBlockGoal;
            case BlockTypes.Pink:
                return pinkBlockGoal;
            case BlockTypes.Purple:
                return purpleBlockGoal;
            case BlockTypes.Red:
                return redBlockGoal;
            case BlockTypes.Yellow:
                return yellowBlockGoal;
            default: 
                return blueBlockGoal;
        }
    }
    
    public Vector3 GetGoalPosition(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Blue:
                return blueBlockGoal.gameObject.transform.position;
            case BlockTypes.Green:
                return greenBlockGoal.gameObject.transform.position;
            case BlockTypes.Pink:
                return pinkBlockGoal.gameObject.transform.position;
            case BlockTypes.Purple:
                return purpleBlockGoal.gameObject.transform.position;
            case BlockTypes.Red:
                return redBlockGoal.gameObject.transform.position;
            case BlockTypes.Yellow:
                return yellowBlockGoal.gameObject.transform.position;
            default: 
                return redBlockGoal.gameObject.transform.position;
        }
    }
}