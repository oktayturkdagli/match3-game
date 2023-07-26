using UnityEngine;
using UnityEngine.UI;

// This class allows the properties of the canvas to change when the screen size of the application changes.
public class UIFitter : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private RectTransform rectTransform;
    
    private float oldScreenRatio;
    private float newScreenRatio;
    private float changePercent;
    private float imageResRatio;
    
    private void Start()
    {
        if (canvasScaler.referenceResolution.x == Screen.width)
            return;

        imageResRatio = rectTransform.sizeDelta.x / rectTransform.sizeDelta.y;
        oldScreenRatio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
        newScreenRatio = (float)Screen.width / (float)Screen.height;
        
        if (oldScreenRatio > newScreenRatio)
            return;
        
        changePercent = CalculateChangeRatio();
        
        float newX = CalculateWidth();
        float newY = CalculateHeight(newX);
        
        rectTransform.sizeDelta = new Vector2(newX, newY);
    }
    
    private float CalculateChangeRatio()
    {
        return ((newScreenRatio - oldScreenRatio) * 100f) / oldScreenRatio;
    }
    
    private float CalculateWidth()
    {
        return rectTransform.sizeDelta.x + (rectTransform.sizeDelta.x * (changePercent / 100f));
    }
    
    private float CalculateHeight(float newX)
    {
        return newX / imageResRatio;
    }
}