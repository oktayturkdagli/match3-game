using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject failMenu;
    
    private void OnEnable()
    {
        LevelManager.levelFailedEvent += OpenFailMenu;
        LevelManager.levelSuccesedEvent += OpenWinMenu;
    }
    
    private void OnDisable()
    {
        LevelManager.levelFailedEvent -= OpenFailMenu;
        LevelManager.levelSuccesedEvent -= OpenWinMenu;
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
