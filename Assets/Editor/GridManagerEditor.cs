using UnityEngine;
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
    
    private BlockTypes[,] blockTypes = new BlockTypes[1, 1];
    private CubeTypes[,] cubeTypes = new CubeTypes[1, 1];

    private void OnEnable()
    {
        GridManager.LoadLevelDataToGridManager();
        gridSizeX = GridManager.gameGrid.GridSizeX;
        gridSizeY = GridManager.gameGrid.GridSizeY;
        movesCount = GridManager.moves;
            
        blueCubeGoal = GridManager.goals.blueCubeCount;
        greenCubeGoal = GridManager.goals.greenCubeCount;
        pinkCubeGoal = GridManager.goals.pinkCubeCount;
        purpleCubeGoal = GridManager.goals.purpleCubeCount;
        redCubeGoal = GridManager.goals.redCubeCount;
        yellowCubeGoal = GridManager.goals.yellowCubeCount;
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
                    GridManager.goals.blueCubeCount = blueCubeGoal;
                    GridManager.goals.greenCubeCount = greenCubeGoal;
                    GridManager.goals.pinkCubeCount = pinkCubeGoal;
                    GridManager.goals.purpleCubeCount = purpleCubeGoal;
                    GridManager.goals.redCubeCount = redCubeGoal;
                    GridManager.goals.yellowCubeCount = yellowCubeGoal;
                    
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
                DisplayGridSizePanel();
                DisplayGroupPanel();
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
    
    private void DisplayGridSizePanel()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("GameGrid Size (X/Y): ", GUILayout.Width(150), GUILayout.Height(15));
        
        gridSizeX = EditorGUILayout.IntField(gridSizeX, EditorStyles.textArea, GUILayout.Width(40));
        gridSizeY = EditorGUILayout.IntField(gridSizeY, EditorStyles.textArea, GUILayout.Width(40));
        
        if (GUILayout.Button("Set Grid", GUILayout.Width(150)))
        {
            GridManager.gameGrid.GridSizeX = gridSizeX;
            GridManager.gameGrid.GridSizeY = gridSizeY;
            
            GridManager.CreateGrid();
            FillBlocksArrays();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Number of Colors: ", GUILayout.Width(150), GUILayout.Height(15));
        numberOfColors = EditorGUILayout.IntField(numberOfColors, EditorStyles.textArea, GUILayout.Width(40));
        if (numberOfColors > 6)
            numberOfColors = 6;
        EditorGUILayout.EndHorizontal();
    }
    
    private void DisplayGroupPanel()
    {
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

        if (blockTypes.GetLength(0) != GridManager.gameGrid.GridSizeX)
        {
            gridSizeX = GridManager.gameGrid.GridSizeX;
            gridSizeY = GridManager.gameGrid.GridSizeY;
            GridManager.CreateGrid();
            FillBlocksArrays();
        }
        
        for (var i = 0; i < gridSizeY; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (var j = 0; j < gridSizeX; j++)
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
        GUILayout.Space(15f);
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Randomly Fill The Grid", GUILayout.Height(30)))
        {
            for (var i = 0; i < gridSizeY; i++)
            {
                for (var j = 0; j < gridSizeX; j++)
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
    
    private void FillBlocksArrays()
    {
        blockTypes = new BlockTypes[gridSizeX, gridSizeY];
        cubeTypes = new CubeTypes[gridSizeX, gridSizeY];
        
        GridManager.TransferLevelDataToGridManager();
        for (var i = 0; i < gridSizeY; i++)
        {
            for (var j = 0; j < gridSizeX; j++)
            {
                blockTypes[j, i] = GridManager.blockTypes[j, i];
                cubeTypes[j, i] = GridManager.cubeTypes[j, i];
            }
        }
    }
    
    private void UpdateCurrentLevelData()
    {
        GridManager.UpdateGridManagerWithExternalData(blockTypes, cubeTypes);
        GridManager.SaveGridData();
        EditorUtility.SetDirty(GridManager.currentLevelData);
        EditorUtility.SetDirty(GridManager);
    }
}