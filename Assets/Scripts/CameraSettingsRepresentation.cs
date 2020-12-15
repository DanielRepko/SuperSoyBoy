using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CameraSettingsRepresentation : MonoBehaviour
{
    public string cameraTrackTarget;
    public float trackingSpeed;
    public float cameraZDepth;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
}
