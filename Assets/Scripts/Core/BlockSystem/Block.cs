using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2 gridIndex;
    public Transform placementPosition;
    [SerializeField] private SpriteRenderer spriteRendererReference;
    
    public BlockTypes blockType;
    public bool canTapped = true;
    
    public static event Action<int, int> OnBlockLeavedGrid;
    
    public void Initialize()
    {
        spriteRendererReference = gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteRendererReference.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        spriteRendererReference.sortingOrder = -(int)gridIndex.y + 1;
        spriteRendererReference.sprite = GameManager.Instance.sharedData.GetBlockSprite(blockType);
    }
    
    public void OnTapped()
    {
        if (!canTapped) 
            return;
        
        // Find same type neighbours
        List<GameObject> sameTypeNeighbours = NeighbourManager.Instance.FindSameTypeNeighbours(blockType, gridIndex);
        if (sameTypeNeighbours.Count < 2) 
            return;
        
        // Do tapped actions
        canTapped = false;
        AudioManager.Instance.PlayBlockExplosionAudio();
        MovesPanel.Instance.Moves -= 1;
        
        // Destroy same type neighbours
        int index = 0;
        foreach (GameObject neighbour in sameTypeNeighbours)
        {
            Block block = neighbour.gameObject.GetComponent<Block>();
            block.canTapped = false;
            EffectsController.Instance.PlayBlockFragmentationEffect(neighbour.transform.position, block.blockType);
            block.placementPosition = null;
            DOTween.Kill(neighbour);
            neighbour.transform.DOKill();

            if (GoalPanel.Instance.CheckIsThereGoal(blockType))
            {
                block.SetSortingLayerName("UI");
                block.SetSortingOrder(10);
                OnBlockLeavedGrid?.Invoke((int)gridIndex.x, (int)gridIndex.y);
                float arriveTime = 0.7f + (index * 0.15f);
                Vector3 goalPosition = GoalPanel.Instance.GetGoalPosition(blockType);
                neighbour.transform.DOMove(goalPosition, arriveTime).SetEase(Ease.InOutBack).OnComplete(() =>
                {
                    AudioManager.Instance.PlayBlockCollectAudio();
                    GoalPanel.Instance.DecreaseGoal(blockType);
                    Destroy(neighbour);
                });
            }
            else
            {
                Destroy(neighbour);
            }
            
            index++;
        }
        
        FillManager.Instance.Fill();
    }
    
    public void MoveToTarget(float arriveTime)
    {
        DOTween.Kill(transform);
        transform.DOKill();
        transform.DOMove(placementPosition.position, arriveTime).SetEase(Ease.OutBounce);
    }
    
    public void UpdateSortingOrder()
    {
        if (!spriteRendererReference)
            spriteRendererReference = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        spriteRendererReference.sortingOrder = -(int)gridIndex.y + 1;
    }

    private void SetSortingLayerName(string layerName)
    {
        if (!spriteRendererReference)
            spriteRendererReference = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        spriteRendererReference.sortingLayerName = layerName;
    }

    private void SetSortingOrder(int index)
    {
        if (!spriteRendererReference)
            spriteRendererReference = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        spriteRendererReference.sortingOrder = index;
    }
}