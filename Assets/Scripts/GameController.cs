using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool gameStarted = false;
    void Start()
    {
        gameStarted = true;
        if(gameStarted)
        {
            //Cursor.visible = false;
        }
    }

    void Update()
    {
        
    }
}
