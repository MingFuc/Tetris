using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBehavior : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //DisplayInGrid();
    }
    void DisplayInGrid()
    {
        if (transform.position.y > MainBoard.instance.topBoundary)
        {
            sr.sortingOrder = -1;
        }
    }
}
