using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallManager : Singleton<FallManager>
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform spawnedBlocksParent;
    [SerializeField] private GameObject blockPrefab;
    
    public void Fall()
    {
        for (var i = 0; i < gridManager.changingColumns.Count; i++)
        {
            var item = gridManager.changingColumns.ElementAt(i);
            var itemKey = item.Key;
            var itemValue = item.Value;
            
            Vector3 startPosition = gridManager.allPositionObjects[itemKey].columns[0].transform.position + new Vector3(0, 2, 0);
            for (var j = 0; j < itemValue; j++)
            {
                Vector3 spawnPosition = startPosition + new Vector3(0, j * 1f, 0);
                int yIndex = (itemValue - 1) - j;
                
                Transform targetTransform = gridManager.allPositionObjects[itemKey].columns[yIndex].transform;
                GameObject spawnedBlockObj = AddRandomBlockToGrid(itemKey, yIndex, spawnPosition, targetTransform);
                
                float arriveTime = Mathf.Clamp(Vector3.Distance(targetTransform.position, spawnPosition)*0.2f,0.5f,0.8f);
                spawnedBlockObj.GetComponent<CubeBlock>().MoveToTarget(arriveTime);
            }
        }
        
        gridManager.changingColumns = new Dictionary<int, int>();
    }
    
    private GameObject AddRandomBlockToGrid(int x, int y, Vector3 spawnPosition, Transform targetTransform)
    {
        GameObject spawnedBlockObj = Instantiate(blockPrefab, spawnPosition, Quaternion.identity, spawnedBlocksParent);
        gridManager.allBlocks[x].columns[y] = spawnedBlockObj;
        CubeTypes currentCubeType = (CubeTypes)Random.Range(0, System.Enum.GetValues(typeof(CubeTypes)).Length);
        CubeBlock currentBlock = spawnedBlockObj.AddComponent<CubeBlock>();
        currentBlock.cubeType = currentCubeType;
        currentBlock.gridIndex = new Vector2(x, y);
        currentBlock.target = targetTransform;
        currentBlock.SetupBlock();
        
        return spawnedBlockObj;
    }
}