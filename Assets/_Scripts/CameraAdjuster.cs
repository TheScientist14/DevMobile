using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    [SerializeField]
    private float maxCameraDistance = 10f;

    // R�solution actuelle de l'�cran
    private Vector2Int currentResolution;

    Camera _mainCamera;

    private void Start()
    {
        // Obtenir la r�solution initiale de l'�cran
        currentResolution = new Vector2Int(Screen.width, Screen.height);
        _mainCamera = Camera.main;
        AdjustCamera();
    }

    private void Update()
    {
        if (Screen.width != currentResolution.x || Screen.height != currentResolution.y)
        {
            currentResolution.x = Screen.width;
            currentResolution.y = Screen.height;

            AdjustCamera();
        }
    }

    void AdjustCamera()
    {
        float targetAspect = (float)Screen.width / (float)Screen.height;
        _mainCamera.aspect = targetAspect;

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float scaleFactor = targetAspect / screenRatio;

        float newOrthographicSize = _mainCamera.orthographicSize * scaleFactor;
        _mainCamera.orthographicSize = newOrthographicSize;

        // Calculer la position max de la cam�ra en fonction de la distance maximale
        Vector3 maxCameraPosition = _mainCamera.transform.position + Vector3.back * maxCameraDistance;

        // V�rifie si la cam�ra d�passe la position maximale
        if (_mainCamera.transform.position.z < maxCameraPosition.z)
        {
            _mainCamera.transform.position = maxCameraPosition;
        }
    }
}
