using System.Collections;
using System;
using TMPro;
using UnityEngine;

public class MovesPanel : Singleton<MovesPanel>
{
    [SerializeField] private TMP_Text movesText;
    private int moves;
    
    public static event Action OnMovesFinished;
    
    public int Moves
    {
        get => moves;
        
        set
        {
            moves = value;
            if (moves <= 0)
            {
                moves = 0;
                StartCoroutine(MovesFinishedDelay());
            }
            movesText.text = moves.ToString();
        }
    }
    
    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += GetMovesData;
    }
    
    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= GetMovesData;
    }

    private void GetMovesData()
    {
        Moves = LevelManager.Instance.CurrentLevelData.moves;
    }
    
    private IEnumerator MovesFinishedDelay()
    {
        yield return new WaitForSeconds(1f);
        OnMovesFinished?.Invoke();
    }
}