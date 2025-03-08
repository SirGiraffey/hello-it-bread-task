using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rendererRef;
    [SerializeField] private ParticleSystem particlesRef;
    [SerializeField] private Sprite explodedMeterSprite;

    private bool breadIsHeld = false;

    public void MeterExplode()
    {
        rendererRef.sprite = explodedMeterSprite;
        particlesRef.Play();
        //zagraj dzwiêk
    }

    public void BreadIsHeld(bool isHeld)
    {
        breadIsHeld = isHeld;
    }

    private void Update()
    {
        if (breadIsHeld)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(2.2f, transform.position.y), 20 * Time.deltaTime);

            float currentScore = GameController.Instance.score;
            if (currentScore > 1) {
                float shakePower = currentScore - 1;
                float shakeRadius = currentScore / 5;
                Vector2 shakeTarget = Vector2.zero + (Random.insideUnitCircle * shakeRadius);
                rendererRef.transform.localPosition = Vector2.Lerp(rendererRef.transform.localPosition, shakeTarget, shakePower * 7 * Time.deltaTime);
                }
            else
            {
                rendererRef.transform.localPosition = Vector2.Lerp(rendererRef.transform.localPosition, Vector2.zero, 30 * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(3.5f, transform.position.y), 30 * Time.deltaTime);
        }
    }
}
