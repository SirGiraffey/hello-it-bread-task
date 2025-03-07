using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandController : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float maxHeight = 0.45f;

    [SerializeField] private BreadController breadController;

    [SerializeField] private int grabState = 0; //0-idle/1-grabempty/2-grabbread

    private SpriteRenderer rendererRef;
    private Rigidbody2D rbRef;
    private Animator animatorRef;
    private ParticleSystem breadParticlesRef;
    private Vector2 mousePosition;

    private float lastHandSpeed = 0f;
    private Vector2 lastDirection = Vector2.zero;

    private bool inFullControl = false;
    private bool isOverBread = false;
    private float handSpeed = 0;
    void Start()
    {
        rendererRef = GetComponentInChildren<SpriteRenderer>();
        animatorRef = GetComponentInChildren<Animator>();
        breadParticlesRef = GetComponentInChildren<ParticleSystem>();
        rbRef = GetComponentInChildren<Rigidbody2D>();
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.y > maxHeight)
        {
            mousePosition = new Vector2(mousePosition.x, maxHeight);
        }

        if (inFullControl)
        {
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, 2), moveSpeed/10 * Time.deltaTime);
            transform.position = Vector2.Lerp(transform.position, new Vector2(mousePosition.x, transform.position.y), moveSpeed * Time.deltaTime);
            if (mousePosition.y <= transform.position.y)
            {
                inFullControl = true;
            }
        }

        if (grabState == 2)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            handSpeed = Mathf.Abs(mouseX) + Mathf.Abs(mouseY);
            GameController.Instance.currentHandSpeed = handSpeed;

            Vector2 currentDirection = new Vector2(mouseX, mouseY).normalized;
            bool suddenSlowdown = handSpeed < lastHandSpeed * 0.5f && lastHandSpeed > 0.5f;

            if (suddenSlowdown)
            {
                PlayBreadParticles(lastDirection, lastHandSpeed);
            }

            lastHandSpeed = handSpeed;
            lastDirection = currentDirection;
        }
        CheckGrabbing();
    }

    void PlayBreadParticles(Vector2 direction, float speed)
    {
        breadParticlesRef.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        ParticleSystem.MainModule main = breadParticlesRef.main;
        main.startSpeedMultiplier = speed;
        StartCoroutine(Particles());
    }

    IEnumerator Particles()
    {
        yield return new WaitForFixedUpdate();
        breadParticlesRef.Play();
    }

    void CheckGrabbing()
    {
        if (!inFullControl)
        {
            return;
        }
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
                GameController.Instance.HandClicked();
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
