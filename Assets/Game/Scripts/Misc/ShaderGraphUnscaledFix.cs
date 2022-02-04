using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderGraphUnscaledFix : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sprite;
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MaterialPropertyBlock myMatBlock = new MaterialPropertyBlock();
        sprite.GetPropertyBlock(myMatBlock);
        myMatBlock.SetFloat("UnscaledTime", Time.unscaledTime);
        sprite.SetPropertyBlock(myMatBlock);
    }
}
