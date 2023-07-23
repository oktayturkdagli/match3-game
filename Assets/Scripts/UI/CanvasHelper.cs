using UnityEngine;
using UnityEngine.Events;

// This class allows the properties of the canvas to change when the screen size of the application changes.
public class CanvasHelper : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform containerTransform;
    
    private bool screenChangeVarsInitialized = false;
    private ScreenOrientation lastOrientation = ScreenOrientation.LandscapeLeft;
    private Vector2 lastResolution = Vector2.zero;
    private Rect lastSafeArea = Rect.zero;
    
    private static UnityEvent OnResolutionOrOrientationChanged = new UnityEvent();
    
    void Awake()
    {

        if (!screenChangeVarsInitialized)
        {
            lastOrientation = Screen.orientation;
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;
            lastSafeArea = Screen.safeArea;

            screenChangeVarsInitialized = true;
        }
        
        ApplySafeArea();
    }
    
    void Update()
    {
        if (Application.isMobilePlatform && Screen.orientation != lastOrientation)
            OrientationChanged();

        if (Screen.safeArea != lastSafeArea)
            SafeAreaChanged();

        if (Screen.width != lastResolution.x || Screen.height != lastResolution.y)
            ResolutionChanged();
    }
    
    private void ApplySafeArea()
    {
        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        containerTransform.anchorMin = anchorMin;
        containerTransform.anchorMax = anchorMax;
    }
    
    private void OrientationChanged()
    {
        lastOrientation = Screen.orientation;
        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;
        
        OnResolutionOrOrientationChanged.Invoke();
    }
    
    private void ResolutionChanged()
    {
        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;
        
        OnResolutionOrOrientationChanged.Invoke();
    }
    
    private void SafeAreaChanged()
    {
        lastSafeArea = Screen.safeArea;
        ApplySafeArea();
    }
}