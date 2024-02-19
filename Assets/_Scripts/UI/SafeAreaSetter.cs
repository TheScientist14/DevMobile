using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaSetter : MonoBehaviour
{

    private Canvas canvas;
    private RectTransform _panelSafeArea;

    private ScreenOrientation currentOrientation;
    private Rect currentSafeArea;


    void Start()
    {
        _panelSafeArea = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        currentOrientation = Screen.orientation;
        currentSafeArea = Screen.safeArea;

        ApplySafeArea();
    }

    private void Update()
    {
        if ((currentOrientation != Screen.orientation) || (currentSafeArea != Screen.safeArea))
            ApplySafeArea();
    }


    void ApplySafeArea()
    {
        if (_panelSafeArea == null) return;

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;

        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        _panelSafeArea.anchorMin = anchorMin;
        _panelSafeArea.anchorMax = anchorMax;
    }
}
