using UnityEngine;
using System;

[Serializable]
public class GameGrid
{
    [SerializeField] private Vector2 gridSize;
    public CubeTypes2DArray[] cubeTypes;
    public BlockTypes2DArray[] blockTypes;
    
    public int GridSizeX
    {
        get => (int)gridSize.x;
        set => gridSize.x = value;
    }
    
    public int GridSizeY
    {
        get => (int)gridSize.y;
        set => gridSize.y = value;
    }
    
    public void CreateGrid()
    {
        // Define the columns of the 2D array
        cubeTypes = new CubeTypes2DArray[(int)gridSize.x];
        blockTypes = new BlockTypes2DArray[(int)gridSize.x];
        
        // Define the rows of the 2D array
        for (var i = 0; i < cubeTypes.Length; i++)
        {
            cubeTypes[i] = new CubeTypes2DArray
            {
                rows = new CubeTypes[(int)gridSize.y]
            };

            blockTypes[i] = new BlockTypes2DArray
            {
                rows = new BlockTypes[(int)gridSize.y]
            };
        } 
    }
    
    public void UpdateGridWithExternalData(BlockTypes[,] blockTypesValue, CubeTypes[,] cubeTypesValue)
    {
        for (var i = 0; i < cubeTypes.Length; i++)
        {
            for (var j = 0; j < cubeTypes[i].rows.Length; j++)
            {
                cubeTypes[i].rows[j] = cubeTypesValue[i, j];
                blockTypes[i].rows[j] = blockTypesValue[i, j];
            }
        }
    }
}