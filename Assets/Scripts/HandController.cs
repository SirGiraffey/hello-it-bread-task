using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandController : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float maxHeight = 0.45f;

    [SerializeField] private BreadController breadController;
    [SerializeField] private TextMeshProUGUI velocityText;

    [SerializeField] private int grabState = 0; //0-idle/1-grabempty/2-grabbread

    private SpriteRenderer rendererRef;
    private Rigidbody2D rbRef;
    private Animator animatorRef;
    private Vector2 mousePosition;

    private bool isOverBread = false;
    private float handSpeed = 0;
    void Start()
    {
        rendererRef = GetComponentInChildren<SpriteRenderer>();
        animatorRef = GetComponentInChildren<Animator>();
        rbRef = GetComponentInChildren<Rigidbody2D>();
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePosition.y > maxHeight)
        {
            mousePosition = new Vector2 (mousePosition.x, maxHeight);
        }
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);

        if (grabState == 2)
        {
            handSpeed = Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));
            velocityText.text = handSpeed.ToString();
            GameController.Instance.currentHandSpeed = handSpeed;
        }
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
            handSpeed = 0;
            GameController.Instance.currentHandSpeed = handSpeed;
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
