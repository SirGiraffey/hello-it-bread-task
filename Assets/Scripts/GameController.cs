using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public bool gameStarted = false;
    public float currentHandSpeed = 0;

    [SerializeField] private GameObject fluidTop;
    [SerializeField] private SpriteRenderer fluidBottomRendererRef;
    [SerializeField] private MeterScript meterScriptRef;
    [SerializeField] private BreadController breadRef;
    [SerializeField] private Color colorRed;
    private Animator fluidAnimatorRef;
    private SpriteRenderer fluidTopRendererRef;
    private float fluidHeight = 0;
    private float fluidExplodeHeight = 2.8f;
    private bool meterExploded = false;

    private bool breadAppearTriggered = false;

    public float score = 0f;

    [SerializeField] private float decayRate = 1f;
    [SerializeField] private float initialDifficulty = 100f;
    [SerializeField] private float difficultyFactor = 10f;

    void Start()
    {
        Application.targetFrameRate = 60;

        fluidAnimatorRef = fluidTop.GetComponent<Animator>();
        fluidTopRendererRef = fluidTop.GetComponent<SpriteRenderer>();
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.visible = !focus;
    }

    public void HandClicked()
    {
        if (!gameStarted && !breadAppearTriggered)
        {
            breadRef.BreadAppear();
            breadAppearTriggered = true;
        }
    }

    void Update()
    {
        if (currentHandSpeed > 0)
        {
            float scoreGain = currentHandSpeed / (initialDifficulty + (Mathf.Sqrt(1 + (score * 100)) * difficultyFactor));
            score += scoreGain;
        }

        score -= decayRate * (1 + score) * Time.deltaTime;
        if (score < 0)
        {
            score = 0;
        }

        fluidAnimatorRef.speed = 1 + fluidHeight;
        fluidHeight = score;
        if (fluidHeight > 5)
        {
            fluidHeight = 5;
        }
        else if (!meterExploded && fluidHeight > fluidExplodeHeight)
        {
            meterScriptRef.MeterExplode();
            meterExploded = true;
        }
        fluidTop.transform.localPosition = new Vector2(0, fluidHeight);
        Color col = Color.Lerp(Color.black, colorRed, score / 3);
        fluidTopRendererRef.color = col;
        fluidBottomRendererRef.color = col;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}