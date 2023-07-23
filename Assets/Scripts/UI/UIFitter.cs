﻿using UnityEngine;
using UnityEngine.UI;

// This class allows the properties of the canvas to change when the screen size of the application changes.
public class UIFitter : Singleton<UIFitter>
{
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private RectTransform rectTransform;
    
    private float oldScreeenRatio;
    private float newScreeenRatio;
    private float changePercent;
    private float imageResRatio;
    
    private void Start()
    {
        if (canvasScaler.referenceResolution.x == Screen.width)
            return;

        imageResRatio = rectTransform.sizeDelta.x / rectTransform.sizeDelta.y;
        oldScreeenRatio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
        newScreeenRatio = (float)Screen.width / (float)Screen.height;
        
        if (oldScreeenRatio > newScreeenRatio)
            return;
        
        changePercent = CalculateChangeRatio();
        
        float newX = CalculateWidth();
        float newY = CalculateHeight(newX);
        
        rectTransform.sizeDelta = new Vector2(newX, newY);
    }
    
    private float CalculateChangeRatio()
    {
        return ((newScreeenRatio - oldScreeenRatio) * 100f) / oldScreeenRatio;
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