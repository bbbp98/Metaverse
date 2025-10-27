using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    [NonSerialized] public Vector2 minPosition;
    [NonSerialized] public Vector2 maxposition;

    [SerializeField] private float smoothSpeed = 0.3f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (target == null)
            return;

        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 pos = transform.position;
        pos = target.position + offset;
        float clampX = pos.x;
        float clampY = pos.y;

        if (SceneManager.GetActiveScene().name == SceneType.LizardRunScene.ToString())
        {
            clampY = 0f;
        }
        else
        {
            clampX = Mathf.Clamp(pos.x, minPosition.x, maxposition.x);
            clampY = Mathf.Clamp(pos.y, minPosition.y, maxposition.y);
        }

        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothSpeed);

        transform.position = new Vector3(clampX, clampY, smoothPos.z);
    }
}
