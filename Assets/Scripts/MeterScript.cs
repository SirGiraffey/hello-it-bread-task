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
            transform.position = Vector2.Lerp(transform.position, new Vector2(2.2f, transform.position.y), 30 * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(3.5f, transform.position.y), 30 * Time.deltaTime);

        }
    }
}
