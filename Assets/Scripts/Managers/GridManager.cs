using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridManager : MonoBehaviour
{
    [SerializeField] public Level currentLevelData;
    [SerializeField] private Transform spawnedBlocksParent;
    [SerializeField] private Transform spawnedPositionObjectsParent;
    [SerializeField] private Transform gridCornerTransform;
    [SerializeField] private GameObject posObjPrefab;
    [SerializeField] private GameObject blockObjPrefab;
    
    [HideInInspector] public GameGrid gameGrid;
    [HideInInspector] public Goals goals;
    [HideInInspector] public int moves;
    [HideInInspector] public BlockTypes[,] blockTypes = new BlockTypes[1, 1];
    [HideInInspector] public CubeTypes[,] cubeTypes = new CubeTypes[1, 1];
    [HideInInspector] public GameObject2DArray[] allBlocks;
    [HideInInspector] public GameObject2DArray[] allPositionObjects;
    
    public Dictionary<int, int> changingColumns = new Dictionary<int, int>(); // Key is column index, Value is changing block count
    
    public Transform SpawnedBlocksParent
    {
        get => spawnedBlocksParent;
        set => spawnedBlocksParent = value;
    }
    
    private void OnEnable()
    {
        CubeBlock.cubeLeavedGridEvent += ClearCell;
    }
    
    private void OnDisable()
    {
        CubeBlock.cubeLeavedGridEvent -= ClearCell;
    }
    
    public void CreateGrid()
    {
        gameGrid.CreateGrid();
    }
    
    public void TransferLevelDataToGridManager()
    {
        // Level Data is transferred to GridManager
        blockTypes = new BlockTypes[gameGrid.GridSizeX, gameGrid.GridSizeY];
        cubeTypes = new CubeTypes[gameGrid.GridSizeX, gameGrid.GridSizeY];
        
        for (var i = 0; i < gameGrid.GridSizeX; i++)
        {
            for (var j = 0; j < gameGrid.GridSizeY; j++)
            {
                blockTypes[i, j] = currentLevelData.GameGrid.blockTypes[i].rows[j];
                cubeTypes[i, j] = currentLevelData.GameGrid.cubeTypes[i].rows[j];
            }
        }
    }
    
    public void UpdateGridManagerWithExternalData(BlockTypes[,] newBlocks, CubeTypes[,] newCubes)
    {
        // GridManager is updated with external data
        blockTypes = new BlockTypes[newBlocks.GetLength(0), newBlocks.GetLength(1)];
        cubeTypes = new CubeTypes[newCubes.GetLength(0), newCubes.GetLength(1)];
        
        for (var i = 0; i < cubeTypes.GetLength(0); i++)
        {
            for (var j = 0; j < cubeTypes.GetLength(1); j++)
            {
                blockTypes[i, j] = newBlocks[i, j];
                cubeTypes[i, j] = newCubes[i, j];
            }
        }
    }
    
    public void LoadLevelDataToGridManager()
    {
        gameGrid = currentLevelData.GameGrid;
        goals = currentLevelData.goals;
        moves = currentLevelData.moves;
        
        TransferLevelDataToGridManager();
    }
    
    public void SaveGridData()
    {
        gameGrid.UpdateGridWithExternalData(blockTypes, cubeTypes);
        currentLevelData.moves = moves;
        SpawnStartingBlocks();
        SetGridCornerSize();
    }
    
    public void SetGridCornerSize()
    {
        float blockSize = 0.755f;
        float gapSize = 0.1f;
        gridCornerTransform.localScale = new Vector3((blockSize * gameGrid.GridSizeX) + (gapSize * (gameGrid.GridSizeX)),
            (blockSize * gameGrid.GridSizeY) + (gapSize * (gameGrid.GridSizeY)) + 0.2f, 1);
    }
    
    public void SpawnStartingBlocks()
    {
        // Clear placeholder objects
        for (var i = spawnedPositionObjectsParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(spawnedPositionObjectsParent.GetChild(i).gameObject);

        // Clear placeholder objects
        for (var i = spawnedBlocksParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(spawnedBlocksParent.GetChild(0).gameObject);
        
        // Define block size and Start Position
        const float blockSize = 0.4f;
        const float gapSize = 0.1f;
        Vector2 startPosition = new Vector2(-(((gameGrid.GridSizeX * blockSize) + ((gameGrid.GridSizeX - 1) * gapSize)) / 2f) + blockSize / 2f,
            (((gameGrid.GridSizeY * blockSize) + ((gameGrid.GridSizeY - 1) * gapSize)) / 2f) - blockSize / 2f);
        
        // Spawn blocks
        allBlocks = new GameObject2DArray[gameGrid.GridSizeX];
        allPositionObjects = new GameObject2DArray[gameGrid.GridSizeX];
        for (var i = 0; i < gameGrid.GridSizeX; i++)
        {
            allBlocks[i] = new GameObject2DArray
            {
                rows = new GameObject[gameGrid.GridSizeY]
            };

            allPositionObjects[i] = new GameObject2DArray
            {
                rows = new GameObject[gameGrid.GridSizeY]
            };

            for (var j = 0; j < gameGrid.GridSizeY; j++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(startPosition.x + (i * blockSize) + (i * gapSize), 
                    startPosition.y - ((j * blockSize) + (j * gapSize)), 0);
                
                // Instantiate block and position objects
                GameObject spawnedPositionObj = Instantiate(posObjPrefab, spawnPosition, Quaternion.identity, spawnedPositionObjectsParent);
                GameObject spawnedBlockObj = Instantiate(blockObjPrefab, spawnPosition, Quaternion.identity,
                    spawnedBlocksParent);
                
                // Define block properties
                DefineBlockProperties(spawnedBlockObj, i, j, spawnedPositionObj.transform);
                allBlocks[i].rows[j] = spawnedBlockObj;
                allPositionObjects[i].rows[j] = spawnedPositionObj;
            }
        }
    }
    
    public void DefineBlockProperties(GameObject blockObj, int xIndex, int yIndex, Transform spawnedPositionTransform)
    {
        BlockTypes currentBlockType = blockTypes[xIndex, yIndex];
        CubeTypes currentCubeType = cubeTypes[xIndex, yIndex];
        
        var blockBase = BlockFactory.GetBlockWithBlockType(currentBlockType);
        Block currentBlock = blockObj.AddComponent(blockBase.GetType()) as Block;
        
        if (blockBase is CubeBlock)
            blockObj.GetComponent<CubeBlock>().cubeType = currentCubeType;
        
        currentBlock.gridIndex = new Vector2(xIndex, yIndex);
        currentBlock.target = spawnedPositionTransform;
        currentBlock.SetupBlock();
    }
    
    public void AddNewChangingColumn(int columnIndex)
    {
        if (changingColumns.ContainsKey(columnIndex))
            changingColumns[columnIndex] += 1;
        else
            changingColumns.Add(columnIndex, 1);
    }
    
    public void DecreaseChangingColumn(int columnIndex)
    {
        if (changingColumns.ContainsKey(columnIndex))
        {
            changingColumns[columnIndex] -= 1;
            if (changingColumns[columnIndex] == 0)
                changingColumns.Remove(columnIndex);
        }
    }
    
    private void ClearCell(int x, int y)
    {
        allBlocks[x].rows[y] = null;
    }
}