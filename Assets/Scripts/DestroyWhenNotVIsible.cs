using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DestroyWhenNotVisible : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private bool _hasBeenVisible;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hasBeenVisible = false;
    }

    private void Update()
    {
        if (_spriteRenderer.isVisible)
        {
            _hasBeenVisible = true;
        }
        else
        {
            if (_hasBeenVisible)
            {
                Destroy(gameObject);
            }
        }
    }
}
