using UnityEngine;

[CreateAssetMenu(fileName ="New Level", menuName ="Level")]
public class Level : ScriptableObject
{
    public Goals goals;
    public GameGrid GameGrid;
    public int numberOfColors;
    public int groupA;
    public int groupB;
    public int groupC;
    public int moves;
}