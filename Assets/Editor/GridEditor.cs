using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    private GridManager gridManager;
    private Vector2 scrollPosition = Vector2.zero;
    
    private string gridSizeXString;
    private string gridSizeYString;
    private string movesCountString;
    private int gridXSize;
    private int gridYSize;
    private int movesCount;
    
    private string blueCubeGoalString;
    private string greenCubeGoalString;
    private string pinkCubeGoalString;
    private string purpleCubeGoalString;
    private string redCubeGoalString;
    private string yellowCubeGoalString;
    
    private int blueCubeGoal;
    private int greenCubeGoal;
    private int pinkCubeGoal;
    private int purpleCubeGoal;
    private int redCubeGoal;
    private int yellowCubeGoal;
    
    private BlockTypes[,] blockTypes = new BlockTypes[1, 1];
    private CubeTypes[,] cubeTypes = new CubeTypes[1, 1];
    
    private bool isExecuted = false;

    private void OnSceneGUI()
    {
        gridManager = (GridManager)target;
        
        if (!isExecuted)
        {
            gridManager.LoadLevelDataToGridManager();
            gridSizeXString = gridManager.gameGrid.GridSizeX.ToString();
            gridSizeYString = gridManager.gameGrid.GridSizeY.ToString();
            movesCountString = gridManager.moves.ToString();
            
            blueCubeGoalString = gridManager.goals.blueCubeCount.ToString();
            greenCubeGoalString = gridManager.goals.greenCubeCount.ToString();
            pinkCubeGoalString = gridManager.goals.pinkCubeCount.ToString();
            purpleCubeGoalString = gridManager.goals.purpleCubeCount.ToString();
            redCubeGoalString = gridManager.goals.redCubeCount.ToString();
            yellowCubeGoalString = gridManager.goals.yellowCubeCount.ToString();
            
            isExecuted = true;
        }
        
        Handles.BeginGUI();
        {
            GUILayout.BeginArea(new Rect(10, Screen.height - 480, 600, 400), new GUIStyle("window"));
            {
                GUIStyle guiStyle = new GUIStyle("BoldLabel");
                guiStyle.fontSize = 15;
                GUILayout.Label(gridManager.gameObject.name, guiStyle, GUILayout.Height(20));
                
                DisplayGridSizePanel();
                GUILayout.Space(15f);
                DisplayGrid();
                GUILayout.Space(15f);
                DisplayButtons();

            }
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(50, 10, 560, 150), new GUIStyle("window"));
            {
                GUIStyle guiStyle = new GUIStyle("BoldLabel");
                guiStyle.fontSize = 15;
                GUILayout.Label("Goal Editor", guiStyle, GUILayout.Height(20));
                DisplayGoals();
                DisplayMovesMenu();
                if (GUILayout.Button("Update Moves Count and Goals For Current Level"))
                {
                    int.TryParse(movesCountString, out var tempMoves);
                    gridManager.moves = tempMoves;
                    
                    int.TryParse(blueCubeGoalString, out blueCubeGoal);
                    gridManager.goals.blueCubeCount = blueCubeGoal;
                    
                    int.TryParse(greenCubeGoalString, out greenCubeGoal);
                    gridManager.goals.greenCubeCount = greenCubeGoal;
                    
                    int.TryParse(pinkCubeGoalString, out pinkCubeGoal);
                    gridManager.goals.pinkCubeCount = pinkCubeGoal;
                    
                    int.TryParse(purpleCubeGoalString, out purpleCubeGoal);
                    gridManager.goals.purpleCubeCount = purpleCubeGoal;
                    
                    int.TryParse(redCubeGoalString, out redCubeGoal);
                    gridManager.goals.redCubeCount = redCubeGoal;
                    
                    int.TryParse(yellowCubeGoalString, out yellowCubeGoal);
                    gridManager.goals.yellowCubeCount = yellowCubeGoal;
                    
                    UpdateCurrentLevelData();
                }
            }
            
            GUILayout.EndArea();
        }
        
        Handles.EndGUI();
    }
    
    private void DisplayGridSizePanel()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("GameGrid Size (X/Y): ", GUILayout.Width(120), GUILayout.Height(15));
        
        gridSizeXString = EditorGUILayout.TextArea(gridSizeXString, EditorStyles.textArea, GUILayout.Width(40));
        gridSizeYString = EditorGUILayout.TextArea(gridSizeYString, EditorStyles.textArea, GUILayout.Width(40));
        
        if (GUILayout.Button("Set Grid", GUILayout.Width(150)))
        {
            int.TryParse(gridSizeXString, out var tempX);
            gridManager.gameGrid.GridSizeX = tempX;

            int.TryParse(gridSizeYString, out var tempY);
            gridManager.gameGrid.GridSizeY = tempY;
            
            gridManager.CreateGrid();
            FillBlocksArrays();
        }
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void DisplayMovesMenu()
    {
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Total Moves: ", GUILayout.Width(80), GUILayout.Height(15));
        movesCountString = EditorGUILayout.TextArea(movesCountString, EditorStyles.textArea, GUILayout.Width(40));
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void FillBlocksArrays()
    {
        gridXSize = gridManager.gameGrid.GridSizeX;
        gridYSize = gridManager.gameGrid.GridSizeY;
        
        gridSizeXString = gridXSize.ToString();
        gridSizeYString = gridYSize.ToString();
        
        blockTypes = new BlockTypes[gridXSize, gridYSize];
        cubeTypes = new CubeTypes[gridXSize, gridYSize];
        
        gridManager.TransferLevelDataToGridManager();
        for (var i = 0; i < gridYSize; i++)
        {
            for (var j = 0; j < gridXSize; j++)
            {
                blockTypes[j, i] = gridManager.blockTypes[j, i];
                cubeTypes[j, i] = gridManager.cubeTypes[j, i];
            }
        }
    }
    
    private void DisplayGoals()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Blue Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        blueCubeGoalString = EditorGUILayout.TextArea(blueCubeGoalString, EditorStyles.textArea, GUILayout.Width(40));
        
        GUILayout.Label("Green Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        greenCubeGoalString = EditorGUILayout.TextArea(greenCubeGoalString, EditorStyles.textArea, GUILayout.Width(40));
        
        GUILayout.Label("Pink Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        pinkCubeGoalString = EditorGUILayout.TextArea(pinkCubeGoalString, EditorStyles.textArea, GUILayout.Width(40));
        
        GUILayout.Label("Purple Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        purpleCubeGoalString = EditorGUILayout.TextArea(purpleCubeGoalString, EditorStyles.textArea, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Red Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        redCubeGoalString = EditorGUILayout.TextArea(redCubeGoalString, EditorStyles.textArea, GUILayout.Width(40));
        
        GUILayout.Label("Yellow Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        yellowCubeGoalString = EditorGUILayout.TextArea(yellowCubeGoalString, EditorStyles.textArea, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
    }
    
    private void DisplayGrid()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        if (blockTypes.GetLength(0) != gridManager.gameGrid.GridSizeX)
            FillBlocksArrays();
        
        for (var i = 0; i < gridYSize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (var j = 0; j < gridXSize; j++)
            {
                EditorGUILayout.BeginVertical();
                GUILayout.Label("Cell " + j + "-" + i, GUILayout.Width(70));
                blockTypes[j, i] = (BlockTypes)EditorGUILayout.EnumPopup(blockTypes[j, i], GUILayout.Width(70));
                
                if (blockTypes[j, i] == BlockTypes.Cube)
                    cubeTypes[j, i] = (CubeTypes)EditorGUILayout.EnumPopup(cubeTypes[j, i], GUILayout.Width(40));
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        GUILayout.EndScrollView();
    }
    
    private void DisplayButtons()
    {
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Randomly Fill The Grid", GUILayout.Height(30)))
        {
            for (var i = 0; i < gridYSize; i++)
            {
                for (var j = 0; j < gridXSize; j++)
                {
                    blockTypes[j, i] = BlockTypes.Cube;
                    CubeTypes randomCube = (CubeTypes) UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(CubeTypes)).Length);
                    cubeTypes[j, i] = randomCube;
                }
            }
            
            UpdateCurrentLevelData();
        }
        
        if (GUILayout.Button("Update Current Level Data", GUILayout.Height(30)))
            UpdateCurrentLevelData();
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void UpdateCurrentLevelData()
    {
        gridManager.UpdateGridManagerWithExternalData(blockTypes, cubeTypes);
        gridManager.SaveGridData();
        EditorUtility.SetDirty(gridManager.currentLevelData);
        EditorUtility.SetDirty(gridManager);
    }
}