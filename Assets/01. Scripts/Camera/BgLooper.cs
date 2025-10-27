using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    const string BackgroundTag = "Background";
    [SerializeField] private int BgCounts = 5;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(BackgroundTag))
        {
            float widthOfBg = ((BoxCollider2D)collision).size.x;
            Vector3 pos = collision.transform.position;

            pos.x += widthOfBg * BgCounts;
            collision.transform.position = pos;

            return;
        }
    }
}
