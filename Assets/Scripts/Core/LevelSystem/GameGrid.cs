using UnityEngine;
using System;

[Serializable]
public class GameGrid
{
    [SerializeField] private Vector2 gridSize;
    public CubeTypes[,] cubeTypes;
    public BlockTypes[,] blockTypes;
    
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
        cubeTypes = new CubeTypes[(int)gridSize.x, (int)gridSize.y];
        blockTypes = new BlockTypes[(int)gridSize.x, (int)gridSize.y];
    }
    
    public void UpdateGridWithExternalData(BlockTypes[,] blockTypesValue, CubeTypes[,] cubeTypesValue)
    {
        for (var i = 0; i < cubeTypes.GetLength(0); i++)
        {
            for (var j = 0; j < cubeTypes.GetLength(1); j++)
            {
                cubeTypes[i, j] = cubeTypesValue[i, j];
                blockTypes[i, j] = blockTypesValue[i, j];
            }
        }
    }
}