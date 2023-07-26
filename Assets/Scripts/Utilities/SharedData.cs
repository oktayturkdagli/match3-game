using UnityEngine;

[CreateAssetMenu(fileName ="SharedData", menuName ="SharedData")]
public class SharedData : ScriptableObject
{
    [Header("Blue")]
    public Sprite blueDefaultSprite;
    public Sprite blueASprite;
    public Sprite blueBSprite;
    public Sprite blueCSprite;
    [Header("Green")]
    public Sprite greenDefaultSprite;
    public Sprite greenASprite;
    public Sprite greenBSprite;
    public Sprite greenCSprite;
    [Header("Pink")]
    public Sprite pinkDefaultSprite;
    public Sprite pinkASprite;
    public Sprite pinkBSprite;
    public Sprite pinkCSprite;
    [Header("Purple")]
    public Sprite purpleDefaultSprite;
    public Sprite purpleASprite;
    public Sprite purpleBSprite;
    public Sprite purpleCSprite;
    [Header("Red")]
    public Sprite redDefaultSprite;
    public Sprite redASprite;
    public Sprite redBSprite;
    public Sprite redCSprite;
    [Header("Yellow")]
    public Sprite yellowDefaultSprite;
    public Sprite yellowASprite;
    public Sprite yellowBSprite;
    public Sprite yellowCSprite;
    
    public Sprite GetBlockSprite(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Blue:
                return blueDefaultSprite;
            case BlockTypes.Green:
                return greenDefaultSprite;
            case BlockTypes.Pink:
                return pinkDefaultSprite;
            case BlockTypes.Purple:
                return purpleDefaultSprite;
            case BlockTypes.Red:
                return redDefaultSprite;
            case BlockTypes.Yellow:
                return yellowDefaultSprite;
        }
        
        return blueDefaultSprite;
    }
}