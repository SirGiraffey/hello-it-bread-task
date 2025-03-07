using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadController : MonoBehaviour
{
    private SpriteRenderer rendererRef;
    private Collider2D colliderRef;

    [SerializeField] private MeterScript meterRef;
    [SerializeField] private float appearSpeed = 10;

    private bool incoming = false;
    void Start()
    {
        rendererRef = GetComponent<SpriteRenderer>();
        colliderRef = GetComponent<Collider2D>();
    }

    public void Update()
    {
        if (incoming)
        {
            transform.position = Vector2.Lerp(transform.position, Vector2.zero, appearSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, Vector2.zero) < 0.01f)
            {
                transform.position = Vector2.zero;
                incoming = false;
            }
        }
    }

    public void BreadAppear()
    {
        incoming = true;
        Debug.Log("appear bread");
    }

    public void Grabbed()
    {
        if (incoming)
        {
            incoming = false;
        }
        GameController.Instance.gameStarted = true;
        rendererRef.enabled = false;
        colliderRef.enabled = false;
        meterRef.BreadIsHeld(true);
    }

    public void Dropped(Vector2 pos)
    {
        Debug.Log("dropped bread");
        if(pos.x > 3 || pos.x < -3)
        {
            pos = new Vector2(3 * Mathf.Sign(pos.x), pos.y);
        }
        if(pos.y < -1.6f)
        {
            pos = new Vector2(pos.x, -1.6f);
        }
        transform.position = pos;
        rendererRef.enabled = true;
        colliderRef.enabled = true;
        meterRef.BreadIsHeld(false);
    }
}
