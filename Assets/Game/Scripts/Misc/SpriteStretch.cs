using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStretch : MonoBehaviour
{
    public bool KeepAspectRatio;

    void Start()
    {
        
    }
    private void Update()
    {
        Camera camera = Camera.main;
        if (camera == null) return;
        float Height = camera.orthographicSize * 2;
        float Width = camera.aspect * Height;

        transform.localScale = new Vector3(Width, Height, 1);
        transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y);
    }
}
