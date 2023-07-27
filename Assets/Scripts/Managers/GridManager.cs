using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridManager : MonoBehaviour
{
    [SerializeField] public Level currentLevel;
    [SerializeField] private Transform spawnedBlocksParent;
    [SerializeField] private Transform spawnedBlockPlacementPositionObjectsParent;
    [SerializeField] private Transform gridCornersParent;
    [SerializeField] private GameObject placementPositionObject;
    [SerializeField] private GameObject blockObject;
    [HideInInspector] public GameObject2DArray[] allBlockObjects;
    [HideInInspector] public GameObject2DArray[] allPlacementPositionObjects;
    
    [HideInInspector] public GameGrid gameGrid;
    [HideInInspector] public Goals goals;
    [HideInInspector] public int moves;
    [HideInInspector] public BlockTypes[,] blockTypes = new BlockTypes[1, 1];
    
    public Dictionary<int, int> incompleteColumnsDictionary = new Dictionary<int, int>(); // Key is column index, Value is changed block count
    
    private void OnEnable()
    {
        Block.OnBlockLeavedGrid += ClearCell;
    }
    
    private void OnDisable()
    {
        Block.OnBlockLeavedGrid -= ClearCell;
    }
    
    public void InitializeGrid()
    {
        gameGrid.Initialize();
    }
    
    public void LoadLevelDataToGridManager()
    {
        gameGrid = currentLevel.gameGrid;
        goals = currentLevel.goals;
        moves = currentLevel.moves;
        
        GetCurrentLevelGridToGridManager();
    }
    
    public void GetCurrentLevelGridToGridManager()
    {
        // Level Data is transferred to GridManager
        blockTypes = new BlockTypes[gameGrid.GridSizeX, gameGrid.GridSizeY];
        
        for (var i = 0; i < gameGrid.GridSizeX; i++)
        {
            for (var j = 0; j < gameGrid.GridSizeY; j++)
            {
                blockTypes[i, j] = currentLevel.gameGrid.blockTypes[i].columns[j];
            }
        }
    }
    
    public void GetExternalGridToGridManager(BlockTypes[,] newBlock)
    {
        // GridManager is updated with external data
        blockTypes = new BlockTypes[newBlock.GetLength(0), newBlock.GetLength(1)];
        
        for (var i = 0; i < blockTypes.GetLength(0); i++)
        {
            for (var j = 0; j < blockTypes.GetLength(1); j++)
            {
                blockTypes[i, j] = newBlock[i, j];
            }
        }
    }
    
    public void SaveGridData()
    {
        gameGrid.GetExternalGridToGrid(blockTypes);
        currentLevel.moves = moves;
        SpawnInitialBlocks();
        SetGridCornerSize();
    }
    
    public void SetGridCornerSize()
    {
        const float blockSize = 0.755f;
        const float gapSize = 0.1f;
        gridCornersParent.localScale = new Vector3((blockSize * gameGrid.GridSizeX) + (gapSize * (gameGrid.GridSizeX)),
            (blockSize * gameGrid.GridSizeY) + (gapSize * (gameGrid.GridSizeY)) + 0.2f, 1);
    }
    
    public void SpawnInitialBlocks()
    {
        // Clear previous objects
        for (var i = spawnedBlockPlacementPositionObjectsParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(spawnedBlockPlacementPositionObjectsParent.GetChild(i).gameObject);
        for (var i = spawnedBlocksParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(spawnedBlocksParent.GetChild(0).gameObject);
        
        // Define block size and Start Position
        const float blockSize = 0.4f;
        const float gapSize = 0.1f;
        Vector2 startPosition = new Vector2(-(((gameGrid.GridSizeX * blockSize) + ((gameGrid.GridSizeX - 1) * gapSize)) / 2f) + blockSize / 2f,
            (((gameGrid.GridSizeY * blockSize) + ((gameGrid.GridSizeY - 1) * gapSize)) / 2f) - blockSize / 2f);
        
        // Spawn blocks
        allBlockObjects = new GameObject2DArray[gameGrid.GridSizeX];
        allPlacementPositionObjects = new GameObject2DArray[gameGrid.GridSizeX];
        for (var i = 0; i < gameGrid.GridSizeX; i++)
        {
            allBlockObjects[i] = new GameObject2DArray
            {
                columns = new GameObject[gameGrid.GridSizeY]
            };

            allPlacementPositionObjects[i] = new GameObject2DArray
            {
                columns = new GameObject[gameGrid.GridSizeY]
            };

            for (var j = 0; j < gameGrid.GridSizeY; j++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(startPosition.x + (i * blockSize) + (i * gapSize), 
                    startPosition.y - ((j * blockSize) + (j * gapSize)), 0);
                
                // Instantiate block and position objects
                GameObject spawnedBlockPlacementPositionObject = Instantiate(placementPositionObject, spawnPosition, Quaternion.identity, spawnedBlockPlacementPositionObjectsParent);
                GameObject spawnedBlockObject = Instantiate(blockObject, spawnPosition, Quaternion.identity, spawnedBlocksParent);
                
                // Define block properties
                Block spawnedBlock = spawnedBlockObject.AddComponent<Block>();
                spawnedBlock.blockType = blockTypes[i, j];
                spawnedBlock.gridIndex = new Vector2(i, j);
                spawnedBlock.placementPosition = spawnedBlockPlacementPositionObject.transform;
                spawnedBlock.Initialize();
                allBlockObjects[i].columns[j] = spawnedBlockObject;
                allPlacementPositionObjects[i].columns[j] = spawnedBlockPlacementPositionObject;
            }
        }
    }
    
    public void AddIncompleteColumn(int columnIndex)
    {
        if (incompleteColumnsDictionary.ContainsKey(columnIndex))
            incompleteColumnsDictionary[columnIndex] += 1;
        else
            incompleteColumnsDictionary.Add(columnIndex, 1);
    }
    
    public void DecreaseIncompleteColumn(int columnIndex)
    {
        if (incompleteColumnsDictionary.ContainsKey(columnIndex))
        {
            incompleteColumnsDictionary[columnIndex] -= 1;
            if (incompleteColumnsDictionary[columnIndex] == 0)
                incompleteColumnsDictionary.Remove(columnIndex);
        }
    }
    
    private void ClearCell(int x, int y)
    {
        allBlockObjects[x].columns[y] = null;
    }
}