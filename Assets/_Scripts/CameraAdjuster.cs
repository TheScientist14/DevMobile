using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    public bool maintainWidth = true;

    [Range(-1f, 1f)]
    public int adaptPosition;

    // R�solution actuelle de l'�cran
    private Vector2Int currentResolution;

    float defaultWidth;
    float defaultHeight;

    Vector3 CameraPos;

    private void Start()
    {
        CameraPos = Camera.main.transform.position;
        defaultHeight = Camera.main.orthographicSize;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;

        // Obtenir la r�solution initiale de l'�cran
        currentResolution = new Vector2Int(Screen.width, Screen.height);

        AdjustCamera();
    }

    private void Update()
    {
        // Si la r�solution change alors on ajuste la cam�ra
        if (Screen.width != currentResolution.x || Screen.height != currentResolution.y)
        {
            currentResolution.x = Screen.width;
            currentResolution.y = Screen.height;

            AdjustCamera();
        }
    }

    void AdjustCamera()
    {
        if (maintainWidth)
        {
            Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;

            Camera.main.transform.position = new Vector3(CameraPos.x, adaptPosition * (defaultHeight - Camera.main.orthographicSize), CameraPos.z);
        }
        else
        {
            Camera.main.transform.position = new Vector3(adaptPosition * (defaultWidth - Camera.main.orthographicSize * Camera.main.aspect), CameraPos.y, CameraPos.z);
        }
    }
}
