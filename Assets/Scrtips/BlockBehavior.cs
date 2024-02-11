using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BlockBehavior : MonoBehaviour
{
    BlockBehavior blockBehavior;

    private float time = 0;
    private float fallTime;

    private bool isCollide = false;
    private bool continueSpawn = false;
    private bool isHoldingS_Key = false; // if player keeps holding S => it wont affect new piece's fall time  

    private bool isHoldingDropButton = false;



    private void Awake()
    {
        fallTime = MainBoard.instance.fallTime;
        blockBehavior = GetComponent<BlockBehavior>();




        //MainBoard.instance.LeftButton.onClick.AddListener(LeftButton);
        //MainBoard.instance.RightButton.onClick.AddListener(RightButton);

        //MainBoard.instance.LeftButton.clicked += LeftButton;
        //MainBoard.instance.RightButton.clicked += RightButton;

    }

    //private void OnEnable()
    //{
    //    MainBoard.instance.leftButtonEvent += LeftButton;
    //    MainBoard.instance.rightButtonEvent += RightButton;
    //}

    //private void OnDisable()
    //{
    //    MainBoard.instance.leftButtonEvent -= LeftButton;
    //    MainBoard.instance.rightButtonEvent -= RightButton;
    //}

    private void Update()
    {
       

        if (MainBoard.instance.isGamePaused == false)
        {
            if (isCollide == false)
            {

                if (Input.GetKeyDown(KeyCode.A) || MainBoard.instance.leftButtonClicked == true)
                {
                    MoveLeft();
                    //prevent collide from rightside
                    for (int i = 0; i < 4; i++)
                    {
                        if (MainBoard.instance.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
                        {
                            MoveRight();
                            MainBoard.instance.leftBlocked = true;
                        }
                    }

                    //MainBoard.instance.leftButtonClicked = false;
                }
                if (Input.GetKeyDown(KeyCode.D) || MainBoard.instance.rightButtonClicked == true)
                {
                    MoveRight();
                    //prevent collide from leftside
                    for (int i = 0; i < 4; i++)
                    {
                        if (MainBoard.instance.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
                        {
                            MoveLeft();
                            MainBoard.instance.rightBlocked = true;
                        }
                    }

                    //MainBoard.instance.rightButtonClicked = false;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    transform.Rotate(0, 0, 90);
                    //prevent collide from rotating (stuck => go upward)
                    for (int i = 0; i < 4; i++)
                    {
                        if (MainBoard.instance.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
                        {

                            gameObject.transform.Translate(new Vector3(0, 1), Space.World);
                            i = -1; //reloop until no blocks overlap

                        }
                    }
                }

                //HardDrop();

                if (Input.GetKeyDown(KeyCode.S) || MainBoard.instance.dropButtonDown == true)
                {
                    Debug.Log(gameObject.name + " start press " + fallTime.ToString());
                    fallTime = fallTime / 10;
                    isHoldingS_Key = true;

                    isHoldingDropButton = true;
                    MainBoard.instance.dropButtonDown = false;
                    Debug.Log(gameObject.name + " endpress " + fallTime.ToString());

                }
                if (((Input.GetKeyUp(KeyCode.S)) && isHoldingS_Key == true)  || (MainBoard.instance.dropButtonUp == true && isHoldingDropButton == true)) //if previously held the S button
                {
                    Debug.Log(gameObject.name + " start release " + fallTime.ToString());
                    fallTime = fallTime * 10;
                    isHoldingS_Key = false;

                    isHoldingDropButton = false;
                    MainBoard.instance.dropButtonUp = false;
                    Debug.Log(gameObject.name + " end release " +fallTime.ToString());
                }
                FallDown();
                CheckBound();
            }
            if (isCollide == true && continueSpawn == true)
            {
                MainBoard.instance.clearLineAndSpawn = true;
                continueSpawn = false;
                blockBehavior.enabled = false;
            }
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

    //void LeftButton()
    //{
    //    if (!MainBoard.instance.isGamePaused == false)
    //        return;
    //    if (!isCollide == false)
    //        return;


    //    MoveLeft();
    //    //prevent collide from rightside
    //    for (int i = 0; i < 4; i++)
    //    {
    //        if (MainBoard.instance.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
    //        {
    //            MoveRight();
    //            MainBoard.instance.leftBlocked = true;
    //        }
    //    }
    //}

    //void RightButton()
    //{
    //    if (!MainBoard.instance.isGamePaused == false)
    //        return;
    //    if (!isCollide == false)
    //        return;


    //    MoveRight();
    //    //prevent collide from leftside
    //    for (int i = 0; i < 4; i++)
    //    {
    //        if (MainBoard.instance.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
    //        {
    //            MoveLeft();
    //            MainBoard.instance.rightBlocked = true;
    //        }
    //    }
    //}


    void HardDrop()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            do
            {
                gameObject.transform.Translate(new Vector2(0, -1), Space.World);
                for (int i = 0; i < 4; i++)
                {
                    if (MainBoard.instance.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
                    {
                        gameObject.transform.Translate(new Vector2(0, 1), Space.World);
                        return;
                    }

                }
            }
            while (true);
        }
    }
    void CheckBound()
    {
        //check collide with left side or right side
        for (int i = 0; i < 4; i++)
        {
            if (Mathf.RoundToInt(transform.GetChild(i).position.x) < 2)
            {
                MoveRight();
                i = -1;

            }
            else if (Mathf.RoundToInt(transform.GetChild(i).position.x) > 11)
            {
                MoveLeft();
                i = -1;

            }
        }

        //check and add grid below
        for (int i = 0; i < 4; i++)
        {
            if (MainBoard.instance.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
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
            MainBoard.instance.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] = 1;

        }
    }
}
