using System.Collections.Generic;
using UnityEngine;

public class NeighbourManager : Singleton<NeighbourManager>
{
    [SerializeField] private GridManager gridManager;
    private List<GameObject> neighbours = new List<GameObject>();
    
    public List<GameObject> FindSameTypeNeighbours(BlockTypes blockType, Vector2 gridIndex)
    {
        neighbours.Clear();
        gridManager.incompleteColumnsDictionary.Clear();
        gridManager.AddIncompleteColumn((int)gridIndex.x);
        neighbours.Add(gridManager.allBlockObjects[(int)gridIndex.x].columns[(int)gridIndex.y]);
        
        RecursiveNeighbourSearch(blockType, gridIndex);
        
        return neighbours;
    }

    private void RecursiveNeighbourSearch(BlockTypes blockType, Vector2 gridIndex)
    {
        var x = (int)gridIndex.x;
        var y = (int)gridIndex.y;
        
        if (x + 1 < gridManager.allBlockObjects.Length)
        {
            var block = gridManager.allBlockObjects[x + 1].columns[y].GetComponent<Block>();
            if (block.blockType == blockType)
            {
                if (!neighbours.Contains(gridManager.allBlockObjects[x + 1].columns[y]))
                {
                    gridManager.AddIncompleteColumn(x + 1);
                    neighbours.Add(gridManager.allBlockObjects[x + 1].columns[y]);
                    RecursiveNeighbourSearch(blockType, new Vector2(x + 1, y));
                }
            }
        }
        
        if (y + 1 < gridManager.allBlockObjects[0].columns.Length)
        {
            var block = gridManager.allBlockObjects[x].columns[y + 1].GetComponent<Block>();
            if (block.blockType == blockType)
            {
                if (!neighbours.Contains(gridManager.allBlockObjects[x].columns[y + 1]))
                {
                    gridManager.AddIncompleteColumn(x);
                    neighbours.Add(gridManager.allBlockObjects[x].columns[y + 1]);
                    RecursiveNeighbourSearch(blockType, new Vector2(x, y + 1));
                }
            }
        }
        
        if (x - 1 >= 0)
        {
            var block = gridManager.allBlockObjects[x - 1].columns[y].GetComponent<Block>();
            if (block.GetComponent<Block>().blockType == blockType)
            {
                if (!neighbours.Contains(gridManager.allBlockObjects[x - 1].columns[y]))
                {
                    gridManager.AddIncompleteColumn(x - 1);
                    neighbours.Add(gridManager.allBlockObjects[x - 1].columns[y]);
                    RecursiveNeighbourSearch(blockType, new Vector2(x - 1, y));
                }
            }
        }
        
        if (y - 1 >= 0)
        {
            var block = gridManager.allBlockObjects[x].columns[y - 1].GetComponent<Block>();
            if (block.blockType == blockType)
            {
                if (!neighbours.Contains(gridManager.allBlockObjects[x].columns[y - 1]))
                {
                    gridManager.AddIncompleteColumn(x);
                    neighbours.Add(gridManager.allBlockObjects[x].columns[y - 1]);
                    RecursiveNeighbourSearch(blockType, new Vector2(x, y - 1));
                }
            }
        }
    }
}