using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    [SerializeField]
    private float maxCameraDistance = 10f;

    // Résolution actuelle de l'écran
    private Vector2Int currentResolution;

    private void Start()
    {
        // Obtenir la résolution initiale de l'écran
        currentResolution = new Vector2Int(Screen.width, Screen.height);

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
        Camera mainCamera = Camera.main;

        float targetAspect = (float)Screen.width / (float)Screen.height;
        mainCamera.aspect = targetAspect;

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float scaleFactor = targetAspect / screenRatio;

        float newOrthographicSize = mainCamera.orthographicSize * scaleFactor;
        mainCamera.orthographicSize = newOrthographicSize;

        // Calculer la position max de la caméra en fonction de la distance maximale
        Vector3 maxCameraPosition = mainCamera.transform.position + Vector3.back * maxCameraDistance;

        // Vérifie si la caméra dépasse la position maximale
        if (mainCamera.transform.position.z < maxCameraPosition.z)
        {
            mainCamera.transform.position = maxCameraPosition;
        }
    }
}
