using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    private float time = 0;
    private float fallTime = 0.5f;

    private bool isCollide = false;
    private bool continueSpawn = false;

    private void Update()
    {
        if (isCollide == false)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
                //prevent collide from rightside
                for (int i = 0; i < 4; i++)
                {
                    if (MainBoard.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
                    {
                        MoveRight();
                        MainBoard.leftBlocked = true;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
                //prevent collide from leftside
                for (int i = 0; i < 4; i++)
                {
                    if (MainBoard.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
                    {
                        MoveLeft();
                        MainBoard.rightBlocked = true;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                transform.Rotate(0, 0, 90);
                //prevent collide from rotating (stuck => go upward)
                for (int i = 0; i < 4; i++)
                {
                    if (MainBoard.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
                    {
                        //int xxx = ((int)transform.position.x - (int)transform.GetChild(i).position.x) / Mathf.Abs(((int)transform.position.x - (int)transform.GetChild(i).position.x)); // value 1 or -1
                        gameObject.transform.Translate(new Vector3(0, 1), Space.World);
                        i = -1; //reloop until no blocks overlap
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fallTime = 0.005f;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                fallTime = 0.05f;
            }
            FallDown();
            CheckBound();
        }
        if (isCollide == true && continueSpawn == true)
        {
            MainBoard.clearLineAndSpawn = true;
            continueSpawn = false;
        }
    }
    void MoveUp()
    {
        gameObject.transform.Translate(new Vector2(0, 1), Space.World);
    }
    void MoveDown()
    {
        gameObject.transform.Translate(new Vector2(0, -1), Space.World);
    }
    void MoveLeft()
    {
        gameObject.transform.Translate(new Vector2(-1, 0), Space.World);
    }
    void MoveRight()
    {
        gameObject.transform.Translate(new Vector2(1, 0), Space.World);
    }
    void CheckBound()
    {
        //check collide with left side or right side
        for (int i = 0; i < 4; i++)
        {
            if (transform.GetChild(i).position.x <= 1)
            {
                MoveRight();
                i = -1;
                //break;
            }
            else if (transform.GetChild(i).position.x >= 12)
            {
                MoveLeft();
                i = -1;
                //break;
            }
        }

        //check and add grid below
        for (int i = 0; i < 4; i++)
        {
            if (MainBoard.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
            {
                MoveUp();
                AddGrid();
                isCollide = true;
                continueSpawn = true;
                break;
            }
        }
    }
    void FallDown()
    {
        time += Time.deltaTime;
        if (time > fallTime)
        {
            MoveDown();
            time = 0;
        }
    }
    void AddGrid()
    {
        for (int i = 0; i < 4; i++)
        {
            MainBoard.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] = 1;
            Debug.Log("[" + Mathf.RoundToInt(transform.GetChild(i).position.x) + "," + Mathf.RoundToInt(transform.GetChild(i).position.y) + "]" + " = 1");
        }
    }
}
