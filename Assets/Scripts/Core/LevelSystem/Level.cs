using UnityEngine;

[CreateAssetMenu(fileName ="New Level", menuName ="Level")]
public class Level : ScriptableObject
{
    public Goals goals;
    public GameGrid GameGrid;
    public int moves;
}