using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject failMenu;
    
    private void OnEnable()
    {
        LevelManager.OnLevelFailed += OpenFailMenu;
        LevelManager.OnLevelSucceed += OpenWinMenu;
    }
    
    private void OnDisable()
    {
        LevelManager.OnLevelFailed -= OpenFailMenu;
        LevelManager.OnLevelSucceed -= OpenWinMenu;
    }
    
    private void OpenWinMenu()
    {
        winMenu.SetActive(true);
    }
    
    private void OpenFailMenu()
    {
        failMenu.SetActive(true);
    }
}
