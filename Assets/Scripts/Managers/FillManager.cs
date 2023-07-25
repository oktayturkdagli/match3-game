using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FillManager : Singleton<FillManager>
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject blockPrefab;
    
    public void Fill()
    {
        for (var i = 0; i < gridManager.changingColumns.Count; i++)
        {
            KeyValuePair<int, int> item = gridManager.changingColumns.ElementAt(i);
            var itemKey = item.Key;
            var itemValue = item.Value;
            
            int rowLength = gridManager.allBlocks.GetLength(1);
            for (var j = rowLength - 1; j >= 0; j--)
            {
                if (gridManager.allBlocks[itemKey, j] == null || (gridManager.allBlocks[itemKey, j] && gridManager.allBlocks[itemKey, j].GetComponent<Block>().target == null))
                {
                    for (var k = j; k >= 0; k--)
                    {
                        if (gridManager.allBlocks[itemKey, k] && gridManager.allBlocks[itemKey, k].GetComponent<Block>().target != null)
                        {
                            GameObject newTargetObj = gridManager.allPositionObjects[itemKey, j].gameObject;
                            Block currentBlock = gridManager.allBlocks[itemKey, k].gameObject.GetComponent<Block>();
                            currentBlock.target = newTargetObj.transform;
                            currentBlock.gridIndex = new Vector2(itemKey, j);
                            
                            gridManager.allBlocks[itemKey, j] = gridManager.allBlocks[itemKey, k];
                            gridManager.allBlocks[itemKey, k] = null;
                            currentBlock.UpdateSortingOrder();
                            currentBlock.MoveToTarget(0.5f);
                            break;
                        }
                    }
                }
            }
        }
        
        FallManager.Instance.Fall();
    }
}