using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CubeBlock : Block
{
    public override BlockTypes blockType => BlockTypes.Cube;
    public override Vector3 spriteSize => new Vector3(0.5f, 0.5f, 0.5f);
    
    private SpriteRenderer mySpriteRenderer;
    public CubeTypes cubeType;
    public bool canTapped = true;
    
    public static event Action<int, int> cubeLeavedGridEvent;

    public override void DoTappedActions()
    {
        if (!canTapped) 
            return;
        
        List<GameObject> sameTypeNeighbours = NeighbourManager.Instance.FindSameTypeNeighbours(cubeType, gridIndex);
        if (sameTypeNeighbours.Count < 2) 
            return;
        
        canTapped = false;
        AudioManager.Instance.PlayCubeExplosionAudio();
        MovesPanel.Instance.Moves -= 1;
        int index = 0;
        
        foreach (GameObject neighbour in sameTypeNeighbours)
        {
            CubeBlock currentBlock = neighbour.gameObject.GetComponent<CubeBlock>();
            currentBlock.canTapped = false;
            EffectsController.Instance.SpawnCubeCrackEffect(neighbour.transform.position, currentBlock.cubeType);
            currentBlock.target = null;
            DOTween.Kill(neighbour);
            neighbour.transform.DOKill();

            if (GoalPanel.Instance.CheckIsThereGoalFromCubeType(cubeType))
            {
                currentBlock.SetSortingLayerName("UI");
                currentBlock.SetSortingOrder(10);
                cubeLeavedGridEvent?.Invoke((int)gridIndex.x, (int)gridIndex.y);
                float arriveTime = 0.7f + (index * 0.15f);
                Vector3 targetPos = GoalPanel.Instance.GetGoalPositionFromCubeType(cubeType);
                neighbour.transform.DOMove(targetPos, arriveTime).SetEase(Ease.InOutBack).OnComplete(() =>
                {
                    AudioManager.Instance.PlayCubeCollectAudio();

                    GoalPanel.Instance.DecreaseGoalFromCubeType(cubeType);
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
    
    public override void SetupBlock()
    {
        mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        mySpriteRenderer.transform.localScale = spriteSize;
        mySpriteRenderer.sortingOrder = -(int)gridIndex.y + 1;
        mySpriteRenderer.sprite = GetMySprite();
    }
    
    public override void MoveToTarget(float arriveTime)
    {
        DOTween.Kill(transform);
        transform.DOKill();
        transform.DOMove(target.position, arriveTime).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            // Do something
        });
    }
    
    public override void UpdateSortingOrder()
    {
        if (!mySpriteRenderer)
            mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        mySpriteRenderer.sortingOrder = -(int)gridIndex.y + 1;
    }
    
    public override void SetSortingLayerName(string layerName)
    {
        if (!mySpriteRenderer)
            mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        mySpriteRenderer.sortingLayerName = layerName;
    }
    
    public override void SetSortingOrder(int index)
    {
        if (!mySpriteRenderer)
            mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        mySpriteRenderer.sortingOrder = index;
    }
    
    private Sprite GetMySprite()
    {
        if (!GameManager.Instance)
            Debug.Log("GameManager.Instance is null");
        
        else if (!GameManager.Instance.sharedData)
            Debug.Log("sharedData is null");
        
        switch (cubeType)
        {
            case CubeTypes.Blue:
                return GameManager.Instance.sharedData.blueCubeBlockSprite;
            case CubeTypes.Green:
                return GameManager.Instance.sharedData.greenCubeBlockSprite;
            case CubeTypes.Pink:
                return GameManager.Instance.sharedData.pinkCubeBlockSprite;
            case CubeTypes.Purple:
                return GameManager.Instance.sharedData.purpleCubeBlockSprite;
            case CubeTypes.Red:
                return GameManager.Instance.sharedData.redCubeBlockSprite;
            case CubeTypes.Yellow:
                return GameManager.Instance.sharedData.yellowCubeBlockSprite;
        }
        
        return GameManager.Instance.sharedData.blueCubeBlockSprite;
    }
}