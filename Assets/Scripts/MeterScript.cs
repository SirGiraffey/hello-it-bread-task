using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rendererRef;
    [SerializeField] private ParticleSystem particlesRef;
    [SerializeField] private Sprite explodedMeterSprite;

    public void MeterExplode()
    {
        rendererRef.sprite = explodedMeterSprite;
        particlesRef.Play();
        //zagraj dzwiêk
    }
}
