using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    [SerializeField] private TransformAnchor _cameraTransformAnchor = default;
    
    private void OnEnable()
    {
        _cameraTransformAnchor.Transform = mainCamera.transform;
    }
}
