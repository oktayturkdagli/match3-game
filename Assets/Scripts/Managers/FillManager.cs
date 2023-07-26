using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FillManager : Singleton<FillManager>
{
    [SerializeField] private GridManager gridManager;
    
    public void Fill()
    {
        for (var i = 0; i < gridManager.changingColumns.Count; i++)
        {
            KeyValuePair<int, int> item = gridManager.changingColumns.ElementAt(i);
            var itemKey = item.Key;
            var itemValue = item.Value;
            
            int rowLength = gridManager.allBlockObjects[itemKey].columns.Length;
            for (var j = rowLength - 1; j >= 0; j--)
            {
                if (gridManager.allBlockObjects[itemKey].columns[j] == null || (gridManager.allBlockObjects[itemKey].columns[j] && gridManager.allBlockObjects[itemKey].columns[j].GetComponent<Block>().placementPosition == null))
                {
                    for (var k = j; k >= 0; k--)
                    {
                        if (gridManager.allBlockObjects[itemKey].columns[k] && gridManager.allBlockObjects[itemKey].columns[k].GetComponent<Block>().placementPosition != null)
                        {
                            GameObject newTargetObj = gridManager.allPlacementPositionObjects[itemKey].columns[j].gameObject;
                            Block currentBlock = gridManager.allBlockObjects[itemKey].columns[k].gameObject.GetComponent<Block>();
                            currentBlock.placementPosition = newTargetObj.transform;
                            currentBlock.gridIndex = new Vector2(itemKey, j);
                            gridManager.allBlockObjects[itemKey].columns[j] = gridManager.allBlockObjects[itemKey].columns[k];
                            gridManager.allBlockObjects[itemKey].columns[k] = null;
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