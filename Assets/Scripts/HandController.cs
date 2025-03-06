using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float maxHeight = 0.45f;
    [SerializeField] private BreadController breadController;

    [SerializeField] private int grabState = 0; //0-idle/1-grabempty/2-grabbread

    private SpriteRenderer rendererRef;
    private Animator animatorRef;
    private Vector2 mousePosition;
    private bool isOverBread = false;
    void Start()
    {
        rendererRef = GetComponentInChildren<SpriteRenderer>();
        animatorRef = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePosition.y > maxHeight)
        {
            mousePosition = new Vector2 (mousePosition.x, maxHeight);
        }
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);

        CheckGrabbing();
    }

    void CheckGrabbing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isOverBread)
            {
                grabState = 2;
                breadController.Grabbed();
            }
            else
            {
                grabState = 1;
            }
            animatorRef.SetInteger("grabState", grabState);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (grabState == 2)
            {
                breadController.Dropped(transform.position);
            }
            grabState = 0;
            animatorRef.SetInteger("grabState", grabState);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bread"))
        {
            isOverBread = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bread"))
        {
            isOverBread = false;
        }
    }
}
