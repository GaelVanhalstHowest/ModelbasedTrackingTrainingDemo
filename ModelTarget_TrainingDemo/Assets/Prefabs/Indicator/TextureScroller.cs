using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureScroller : MonoBehaviour
{
    public Vector2 ScrollSpeed = new Vector2(1, 0);

    //Texture _texture;
    Material _material;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
        if (_material == null)
            enabled = false;
        //_texture = _material.GetTexture(0);
    }

    void Update()
    {
        var offset = ScrollSpeed * Time.time;
        _material.SetTextureOffset("_MainTex", offset);
    }
}
