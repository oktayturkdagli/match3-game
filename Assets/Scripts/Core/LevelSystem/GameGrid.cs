using UnityEngine;
using System;

[Serializable]
public class GameGrid
{
    [SerializeField] private Vector2 gridSize;
    public Blocks2DArray[] blockTypes;

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
    
    public void Initialize()
    {
        // Define the columns of the 2D array
        blockTypes = new Blocks2DArray[(int)gridSize.x];

        // Define the rows of the 2D array
        for (var i = 0; i < blockTypes.Length; i++)
        {
            blockTypes[i] = new Blocks2DArray
            {
                columns = new BlockTypes[(int)gridSize.y]
            };
        } 
    }
    
    public void GetExternalGridToGrid(BlockTypes[,] cubeTypesValue)
    {
        for (var i = 0; i < blockTypes.Length; i++)
        {
            for (var j = 0; j < blockTypes[i].columns.Length; j++)
            {
                blockTypes[i].columns[j] = cubeTypesValue[i, j];
            }
        }
    }
}