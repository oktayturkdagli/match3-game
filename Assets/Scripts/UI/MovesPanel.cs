using System.Collections;
using System;
using TMPro;
using UnityEngine;

public class MovesPanel : Singleton<MovesPanel>
{
    [SerializeField] private TMP_Text movesText;
    private int moves;
    
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
    
    public static event Action movesFinishedEvent;
    
    private void OnEnable()
    {
        LevelManager.levelLoadedEvent += GetMovesData;
    }
    
    private void OnDisable()
    {
        LevelManager.levelLoadedEvent -= GetMovesData;
    }

    private void GetMovesData()
    {
        Moves = LevelManager.Instance.CurrentLevelData.moves;
    }
    
    private IEnumerator MovesFinishedDelay()
    {
        yield return new WaitForSeconds(1f);
        movesFinishedEvent?.Invoke();
    }
}