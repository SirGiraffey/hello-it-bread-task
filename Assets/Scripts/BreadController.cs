using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadController : MonoBehaviour
{

    private SpriteRenderer rendererRef;
    private Collider2D colliderRef;

    void Start()
    {
        rendererRef = GetComponent<SpriteRenderer>();
        colliderRef = GetComponent<Collider2D>();
    }

    public void Grabbed()
    {
        rendererRef.enabled = false;
        colliderRef.enabled = false;
    }

    public void Dropped(Vector2 pos)
    {
        Debug.Log("dropped bread");
        transform.position = pos;
        rendererRef.enabled = true;
        colliderRef.enabled = true;
    }
}
