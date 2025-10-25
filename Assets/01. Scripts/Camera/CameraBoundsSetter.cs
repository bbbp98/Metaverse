using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraBoundsSetter : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Tilemap tilemap;

    private void Start()
    {
        if (cameraFollow == null || tilemap == null)
            return;

        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * Camera.main.aspect;

        Vector3 offSet = new Vector3(halfWidth, halfHeight);
        Bounds bounds = tilemap.localBounds;
        cameraFollow.minPosition = bounds.min + offSet;
        cameraFollow.maxposition = bounds.max - offSet;
        cameraFollow.maxposition.y -= tilemap.cellSize.y * 0.75f;
    }
}
