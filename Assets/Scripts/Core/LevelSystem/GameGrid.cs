using UnityEngine;
using System;

[Serializable]
public class GameGrid
{
    [SerializeField] private Vector2 gridSize;
    public CubeTypes2DArray[] cubeTypes;

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

        // Define the rows of the 2D array
        for (var i = 0; i < cubeTypes.Length; i++)
        {
            cubeTypes[i] = new CubeTypes2DArray
            {
                columns = new CubeTypes[(int)gridSize.y]
            };
        } 
    }
    
    public void UpdateGridWithExternalData(CubeTypes[,] cubeTypesValue)
    {
        for (var i = 0; i < cubeTypes.Length; i++)
        {
            for (var j = 0; j < cubeTypes[i].columns.Length; j++)
            {
                cubeTypes[i].columns[j] = cubeTypesValue[i, j];
            }
        }
    }
}