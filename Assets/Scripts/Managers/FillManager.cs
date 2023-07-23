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
            var item = gridManager.changingColumns.ElementAt(i);
            var itemKey = item.Key;
            var itemValue = item.Value;
            
            int rowLength = gridManager.allBlocks[itemKey].rows.Length;
            for (var j = rowLength - 1; j >= 0; j--)
            {
                if (gridManager.allBlocks[itemKey].rows[j] == null || (gridManager.allBlocks[itemKey].rows[j] && gridManager.allBlocks[itemKey].rows[j].GetComponent<Block>().target == null))
                {
                    for (var k = j; k >= 0; k--)
                    {
                        if (gridManager.allBlocks[itemKey].rows[k] && gridManager.allBlocks[itemKey].rows[k].GetComponent<Block>().target != null)
                        {
                            GameObject newTargetObj = gridManager.allPositionObjects[itemKey].rows[j].gameObject;
                            Block currentBlock = gridManager.allBlocks[itemKey].rows[k].gameObject.GetComponent<Block>();
                            currentBlock.target = newTargetObj.transform;
                            currentBlock.gridIndex = new Vector2(itemKey, j);
                            
                            gridManager.allBlocks[itemKey].rows[j] = gridManager.allBlocks[itemKey].rows[k];
                            gridManager.allBlocks[itemKey].rows[k] = null;
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
    
    public void FillOnlyOneBlock(BlockTypes blockType, Vector2 gridIndex)
    {
        int x = (int)gridIndex.x;
        int y = (int)gridIndex.y;
        Vector3 spawnPos = gridManager.allPositionObjects[(int)gridIndex.x].rows[(int)gridIndex.y].transform.position;
        GameObject spawnedBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, gridManager.SpawnedBlocksParent);
        gridManager.allBlocks[x].rows[y] = spawnedBlock;
        
        BlockTypes currentBlockType = blockType;
        
        var myBlockType = BlockFactory.GetBlockWithBlockType(currentBlockType);
        Block currentBlock = spawnedBlock.AddComponent(myBlockType.GetType()) as Block;
        
        currentBlock.gridIndex = gridIndex;
        currentBlock.target = gridManager.allPositionObjects[x].rows[y].transform;
        currentBlock.SetupBlock();
        
        gridManager.DecreaseChangingColumn((int)gridIndex.x);
    }
}