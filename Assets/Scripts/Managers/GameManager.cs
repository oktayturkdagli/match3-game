public class GameManager : Singleton<GameManager>
{
    public SharedData sharedData;
    
    private void Awake()
    {
        Utility.CallASingleton<AudioManager>();
        Utility.CallASingleton<EffectsController>();
        Utility.CallASingleton<FallManager>();
        Utility.CallASingleton<FillManager>();
        Utility.CallASingleton<InputManager>();
        Utility.CallASingleton<LevelManager>();
        Utility.CallASingleton<NeighbourManager>();
        
        Utility.CallASingleton<MovesPanel>();
        Utility.CallASingleton<GoalPanel>();
        
        Utility.CallASingleton<UIManager>();
        Utility.CallASingleton<UIFitter>();
    }
}