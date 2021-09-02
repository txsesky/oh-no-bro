using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;

    private void OnEnable()
    {
        RuntimeData.cameraTransformAnchor = mainCamera.transform;
    }
}
