using System;
using UnityEngine;

// This script is used to display custom 2D arrays in Unity Inspector
[Serializable]
public class BlockTypes2DArray
{
    public BlockTypes[] columns = new BlockTypes[1];
}

[Serializable]
public class CubeTypes2DArray
{
    public CubeTypes[] columns = new CubeTypes[1];
}

[Serializable]
public class GameObject2DArray
{
    public GameObject[] columns = new GameObject[1];
}