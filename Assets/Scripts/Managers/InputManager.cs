using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private bool canClick = false;
    
    private void OnEnable()
    {
        LevelManager.levelLoadedEvent += EnableClicking;
        LevelManager.levelSuccesedEvent += DisableClicking;
        LevelManager.levelFailedEvent += DisableClicking;
    }
    
    private void OnDisable()
    {
        LevelManager.levelLoadedEvent -= EnableClicking;
        LevelManager.levelSuccesedEvent -= DisableClicking;
        LevelManager.levelFailedEvent -= DisableClicking;
    }
    
    void Update()
    {
        #if UNITY_EDITOR
        GetEditorInputs();
        #else
		GetMobileTouches();
        #endif
    }
    
    private void GetEditorInputs()
    {
        if (Input.GetMouseButtonUp(0) && canClick)
            DetectHitObject(Input.mousePosition);
    }
    
    private void GetMobileTouches()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase.Equals(TouchPhase.Ended) && canClick)
            DetectHitObject(touch.position);
    }
    
    private void DetectHitObject(Vector3 touchedPosition)
    {
        if (MovesPanel.Instance.Moves > 0)
        {
            BoxCollider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touchedPosition)) as BoxCollider2D;
            if (hitCollider)
            {
                GameObject hitObj = hitCollider.gameObject;
                if (hitObj.CompareTag("Block"))
                    hitObj.gameObject.GetComponent<CubeBlock>().DoTappedActions();
            }
        } 
    }
    
    private void EnableClicking()
    {
        if (LevelManager.Instance.isLevelActive)
            canClick = true;
    }
    
    private void DisableClicking()
    {
        canClick = false;
    }
}