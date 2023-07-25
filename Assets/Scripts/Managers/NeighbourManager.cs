using System.Collections.Generic;
using UnityEngine;

public class NeighbourManager : Singleton<NeighbourManager>
{
    [SerializeField] private GridManager gridManager;
    List<GameObject> neighbours = new List<GameObject>();
    
    public List<GameObject> FindSameTypeNeighbours(CubeTypes cubeType, Vector2 gridIndex)
    {
        neighbours = new List<GameObject>();

        gridManager.changingColumns = new Dictionary<int, int>();
        gridManager.AddNewChangingColumn((int)gridIndex.x);
        neighbours.Add(gridManager.allBlocks[(int)gridIndex.x].columns[(int)gridIndex.y]);
        
        RecursiveNeighbourSearch(cubeType, gridIndex);
        
        return neighbours;
    }
    
    public void RecursiveNeighbourSearch(CubeTypes cubeType, Vector2 gridIndex)
    {
        int x = (int)gridIndex.x;
        int y = (int)gridIndex.y;
        
        if (x + 1 < gridManager.allBlocks.Length)
        {
            CubeBlock curBlock = gridManager.allBlocks[x + 1].columns[y].GetComponent<CubeBlock>();
            if (curBlock.cubeType == cubeType)
            {
                if (!neighbours.Contains(gridManager.allBlocks[x + 1].columns[y]))
                {
                    gridManager.AddNewChangingColumn(x + 1);
                    neighbours.Add(gridManager.allBlocks[x + 1].columns[y]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x + 1, y));
                }
            }
        }
        
        if (y + 1 < gridManager.allBlocks[0].columns.Length)
        {
            CubeBlock curBlock = gridManager.allBlocks[x].columns[y + 1].GetComponent<CubeBlock>();
            if (curBlock.cubeType == cubeType)
            {
                if (!neighbours.Contains(gridManager.allBlocks[x].columns[y + 1]))
                {
                    gridManager.AddNewChangingColumn(x);
                    neighbours.Add(gridManager.allBlocks[x].columns[y + 1]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x, y + 1));
                }
            }
        }
        
        if (x - 1 >= 0)
        {
            CubeBlock curBlock = gridManager.allBlocks[x - 1].columns[y].GetComponent<CubeBlock>();
            if (curBlock.GetComponent<CubeBlock>().cubeType == cubeType)
            {
                if (!neighbours.Contains(gridManager.allBlocks[x - 1].columns[y]))
                {
                    gridManager.AddNewChangingColumn(x - 1);
                    neighbours.Add(gridManager.allBlocks[x - 1].columns[y]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x - 1, y));
                }
            }
        }
        
        if (y - 1 >= 0)
        {
            CubeBlock curBlock = gridManager.allBlocks[x].columns[y - 1].GetComponent<CubeBlock>();
            if (curBlock.cubeType == cubeType)
            {
                if (!neighbours.Contains(gridManager.allBlocks[x].columns[y - 1]))
                {
                    gridManager.AddNewChangingColumn(x);
                    neighbours.Add(gridManager.allBlocks[x].columns[y - 1]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x, y - 1));
                }
            }
        }
    }
    
    public List<GameObject> FindUpBlocks(Vector2 gridIndex)
    {
        List<GameObject> upBlocks = new List<GameObject>();
        for (var i = 0; i < (int)gridIndex.y; i++)
        {
            gridManager.AddNewChangingColumn((int)gridIndex.x);
            upBlocks.Add(gridManager.allBlocks[(int)gridIndex.x].columns[i]);
        }
        return upBlocks;
    }
    
    public List<GameObject> FindDownBlocks(Vector2 gridIndex)
    {
        List<GameObject> downBlocks = new List<GameObject>();
        for (int i = (int)gridIndex.y + 1; i < gridManager.gameGrid.GridSizeY; i++)
        {
            gridManager.AddNewChangingColumn((int)gridIndex.x);
            downBlocks.Add(gridManager.allBlocks[(int)gridIndex.x].columns[i]);
        }
        return downBlocks;
    }
    
    public List<GameObject> FindRightBlocks(Vector2 gridIndex)
    {
        List<GameObject> rightBlocks = new List<GameObject>();
        for (int i = (int)gridIndex.x + 1; i < gridManager.gameGrid.GridSizeX; i++)
        {
            gridManager.AddNewChangingColumn(i);
            rightBlocks.Add(gridManager.allBlocks[i].columns[(int)gridIndex.y]);
        }
        return rightBlocks;
    }
    
    public List<GameObject> FindLeftBlocks(Vector2 gridIndex)
    {
        List<GameObject> leftBlocks = new List<GameObject>();
        for (int i = 0; i < (int)gridIndex.x; i++)
        {
            gridManager.AddNewChangingColumn(i);
            leftBlocks.Add(gridManager.allBlocks[i].columns[(int)gridIndex.y]);
        }
        return leftBlocks;
    }
}