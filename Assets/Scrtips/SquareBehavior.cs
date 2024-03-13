using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBehavior : MonoBehaviour
{
    private SpriteRenderer sr;



    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
      
    }

    void Update()
    {
        DisplayInGrid();
        CheckGameState();
    }
    void DisplayInGrid()
    {
        if (transform.position.y >= MainBoard.instance.topBoundary)
        {
            if (MainBoard.instance.isGameOver == true)
                gameObject.SetActive(false);
            else
            {
                sr.sortingOrder = -3;
            }
        }
        else
            sr.sortingOrder = 2;
    }
    void CheckGameState()
    {
        if (MainBoard.instance.clearLineAndSpawn == true && Mathf.RoundToInt(gameObject.transform.position.y) >= MainBoard.instance.topBoundary)
        {
            MainBoard.instance.isGameOver = true;
            MainBoard.instance.onLose_Invoke();
        }
    }
}
