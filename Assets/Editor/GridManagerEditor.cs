﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private GridManager GridManager => target as GridManager;
    private Vector2 scrollPosition = Vector2.zero;

    private int gridSizeX = 9;
    private int gridSizeY = 9;
    private int movesCount = 30;
    private int numberOfColors = 6;
    
    private int groupA = 5;
    private int groupB = 7;
    private int groupC = 9;
    
    private int blueCubeGoal = 10;
    private int greenCubeGoal = 10;
    private int pinkCubeGoal = 0;
    private int purpleCubeGoal = 0;
    private int redCubeGoal = 0;
    private int yellowCubeGoal = 0;
    
    private BlockTypes[,] cubeTypes = new BlockTypes[1, 1];

    private void OnEnable()
    {
        GridManager.LoadLevelDataToGridManager();
        gridSizeX = GridManager.gameGrid.GridSizeX;
        gridSizeY = GridManager.gameGrid.GridSizeY;
        numberOfColors = GridManager.numberOfColors;
        groupA = GridManager.groupA;
        groupB = GridManager.groupB;
        groupC = GridManager.groupC;
        movesCount = GridManager.moves;
            
        blueCubeGoal = GridManager.goals.blueBlockCount;
        greenCubeGoal = GridManager.goals.greenBlockCount;
        pinkCubeGoal = GridManager.goals.pinkBlockCount;
        purpleCubeGoal = GridManager.goals.purpleBlockCount;
        redCubeGoal = GridManager.goals.redBlockCount;
        yellowCubeGoal = GridManager.goals.yellowBlockCount;
    }
    
    private void OnSceneGUI()
    {
        Handles.BeginGUI();
        {
            // Display the Goal menu
            GUILayout.BeginArea(new Rect(10, 10, 300, 165), new GUIStyle("window"));
            {
                GUIStyle guiStyle = new GUIStyle("BoldLabel");
                guiStyle.fontSize = 15;
                GUILayout.Label("Goal Editor", guiStyle);
                DisplayGoals();
                DisplayMovesMenu();
                if (GUILayout.Button("Update Moves Count and Goals For Current Level"))
                {
                    GridManager.moves = movesCount;
                    GridManager.goals.blueBlockCount = blueCubeGoal;
                    GridManager.goals.greenBlockCount = greenCubeGoal;
                    GridManager.goals.pinkBlockCount = pinkCubeGoal;
                    GridManager.goals.purpleBlockCount = purpleCubeGoal;
                    GridManager.goals.redBlockCount = redCubeGoal;
                    GridManager.goals.yellowBlockCount = yellowCubeGoal;
                    
                    UpdateCurrentLevelData();
                }
            }
            GUILayout.EndArea();
            
            // Display the grid Manager menu
            GUILayout.BeginArea(new Rect(10, Screen.height - 480, 640, 400), new GUIStyle("window"));
            {
                GUIStyle guiStyle = new GUIStyle("BoldLabel");
                guiStyle.fontSize = 15;
                GUILayout.Label(GridManager.gameObject.name, guiStyle, GUILayout.Height(20));
                DisplayGridHeaderPanel();
                DisplayGrid();
                DisplayButtons();
            }
            GUILayout.EndArea();
        }
        Handles.EndGUI();
    }
    
    private void DisplayGoals()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Blue Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        blueCubeGoal = EditorGUILayout.IntField(blueCubeGoal, EditorStyles.textArea, GUILayout.Width(40));
        
        GUILayout.Label("Green Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        greenCubeGoal = EditorGUILayout.IntField(greenCubeGoal, EditorStyles.textArea, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Pink Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        pinkCubeGoal = EditorGUILayout.IntField(pinkCubeGoal, EditorStyles.textArea, GUILayout.Width(40));
        
        GUILayout.Label("Purple Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        purpleCubeGoal = EditorGUILayout.IntField(purpleCubeGoal, EditorStyles.textArea, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Red Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        redCubeGoal = EditorGUILayout.IntField(redCubeGoal, EditorStyles.textArea, GUILayout.Width(40));
        
        GUILayout.Label("Yellow Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        yellowCubeGoal = EditorGUILayout.IntField(yellowCubeGoal, EditorStyles.textArea, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
    }
    
    private void DisplayMovesMenu()
    {
        GUILayout.Space(15f);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Total Moves: ", GUILayout.Width(80), GUILayout.Height(15));
        movesCount = EditorGUILayout.IntField(movesCount, EditorStyles.textArea, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
    }
    
    private void DisplayGridHeaderPanel()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("GameGrid Size (X/Y): ", GUILayout.Width(150), GUILayout.Height(15));
        
        gridSizeX = EditorGUILayout.IntField(gridSizeX, EditorStyles.textArea, GUILayout.Width(40));
        gridSizeY = EditorGUILayout.IntField(gridSizeY, EditorStyles.textArea, GUILayout.Width(40));
        
        if (GUILayout.Button("Set Grid", GUILayout.Width(150)))
        {
            GridManager.gameGrid.GridSizeX = gridSizeX;
            GridManager.gameGrid.GridSizeY = gridSizeY;
            
            GridManager.numberOfColors = numberOfColors;
            GridManager.groupA = groupA;
            GridManager.groupB = groupB;
            GridManager.groupC = groupC;
            
            GridManager.InitializeGrid();
            FillBlocksArrays();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Number of Colors: ", GUILayout.Width(150), GUILayout.Height(15));
        numberOfColors = EditorGUILayout.IntField(numberOfColors, EditorStyles.textArea, GUILayout.Width(40));
        if (numberOfColors > 6)
            numberOfColors = 6;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Group Settings (A/B/C): ", GUILayout.Width(150), GUILayout.Height(15));
        groupA = EditorGUILayout.IntField(groupA, EditorStyles.textArea, GUILayout.Width(40));
        groupB = EditorGUILayout.IntField(groupB, EditorStyles.textArea, GUILayout.Width(40));
        groupC = EditorGUILayout.IntField(groupC, EditorStyles.textArea, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
    }

    private void DisplayGrid()
    {
        GUILayout.Space(15f);
        
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        if (cubeTypes.GetLength(0) != GridManager.gameGrid.GridSizeX)
            FillBlocksArrays();
        
        for (var i = 0; i < gridSizeY; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (var j = 0; j < gridSizeX; j++)
            {
                EditorGUILayout.BeginVertical();
                GUILayout.Label("Cell " + j + "-" + i, GUILayout.Width(70));
                cubeTypes[j, i] = (BlockTypes)EditorGUILayout.EnumPopup(cubeTypes[j, i], GUILayout.Width(40));
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        
        GUILayout.EndScrollView();
    }
    
    private void DisplayButtons()
    {
        GUILayout.Space(15f);
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Randomly Fill The Grid", GUILayout.Height(30)))
        {
            for (var i = 0; i < gridSizeY; i++)
            {
                for (var j = 0; j < gridSizeX; j++)
                {
                    BlockTypes randomBlock = (BlockTypes)Random.Range(0, numberOfColors);
                    cubeTypes[j, i] = randomBlock;
                }
            }
            
            UpdateCurrentLevelData();
        }
        
        if (GUILayout.Button("Update Current Level Data", GUILayout.Height(30)))
            UpdateCurrentLevelData();
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void FillBlocksArrays()
    {
        cubeTypes = new BlockTypes[gridSizeX, gridSizeY];
        
        GridManager.GetCurrentLevelGridToGridManager();
        for (var i = 0; i < gridSizeY; i++)
        {
            for (var j = 0; j < gridSizeX; j++)
            {
                cubeTypes[j, i] = GridManager.blockTypes[j, i];
            }
        }
    }
    
    private void UpdateCurrentLevelData()
    {
        
        GridManager.GetExternalGridToGridManager(cubeTypes);
        GridManager.SaveGridManagerToLevelData();
        EditorUtility.SetDirty(GridManager.currentLevel);
        EditorUtility.SetDirty(GridManager);
    }
}