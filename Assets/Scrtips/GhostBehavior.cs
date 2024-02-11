using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{

    private bool isCollide = false;

   

    private void Awake()
    {
        MainBoard.instance.onDestroyGhostBlock += DestroyThis;



        //MainBoard.instance.LeftButton.onClick?.AddListener(LeftButton);
        //MainBoard.instance.RightButton.onClick?.AddListener(RightButton);

        //MainBoard.instance.LeftButton.clicked += LeftButton;
        //MainBoard.instance.RightButton.clicked += RightButton;
    }



    private void OnDestroy()
    {
        MainBoard.instance.onDestroyGhostBlock -= DestroyThis;
    }
    private void Update()
    {
       

        if (MainBoard.instance.isGamePaused == false)
        {
            if (MainBoard.instance.isGameOver == false)
            {
                //sync norm and ghost
                if (MainBoard.instance.leftBlocked == true)
                {
                    MoveRight();
                    MainBoard.instance.leftBlocked = false;
                }
                if (MainBoard.instance.rightBlocked == true)
                {
                    MoveLeft();
                    MainBoard.instance.rightBlocked = false;
                }
                //

                if (isCollide == false)
                {
                    FallDown();
                }
                if (Input.GetKeyDown(KeyCode.A) || MainBoard.instance.leftButtonClicked == true)
                {
                    isCollide = false;
                    MoveLeft();

                    while (MainBoard.instance.CheckVertically() == true)
                    {
                        gameObject.transform.Translate(new Vector2(0, MainBoard.instance.ghostMoveUpRange), Space.World);
                    }

                    MainBoard.instance.leftButtonClicked = false;

                }
                if (Input.GetKeyDown(KeyCode.D) || MainBoard.instance.rightButtonClicked == true)
                {
                    isCollide = false;
                    MoveRight();


                    while (MainBoard.instance.CheckVertically() == true)
                    {
                        gameObject.transform.Translate(new Vector2(0, MainBoard.instance.ghostMoveUpRange), Space.World);
                    }

                    MainBoard.instance.rightButtonClicked = false;

                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    isCollide = false;
                    transform.Rotate(0, 0, 90);
                    while (MainBoard.instance.CheckVertically() == true)
                    {
                        gameObject.transform.Translate(new Vector2(0, MainBoard.instance.ghostMoveUpRange), Space.World);
                    }
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
                CheckBound();
            }
            else
                DestroyThis();
        }
    }
    void FallDown()
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
        //check grid below
        for (int i = 0; i < 4; i++)
        {
            if (MainBoard.instance.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
            {
                MoveUp();
                isCollide = true;
                break;
            }
        }
    }
    void DestroyThis()
    {
        gameObject.SetActive(false);
    }

    //void LeftButton()
    //{
    //    if (!MainBoard.instance.isGamePaused == false)
    //        return;
    //    if (!MainBoard.instance.isGameOver == false)
    //        return;

    //    isCollide = false;
    //    MoveLeft();

    //    while (MainBoard.instance.CheckVertically() == true)
    //    {
    //        gameObject.transform.Translate(new Vector2(0, MainBoard.instance.ghostMoveUpRange), Space.World);
    //    }
    //}

    //void RightButton()
    //{
    //    if (!MainBoard.instance.isGamePaused == false)
    //        return;
    //    if (!MainBoard.instance.isGameOver == false)
    //        return;

    //    isCollide = false;
    //    MoveRight();


    //    while (MainBoard.instance.CheckVertically() == true)
    //    {
    //        gameObject.transform.Translate(new Vector2(0, MainBoard.instance.ghostMoveUpRange), Space.World);
    //    }
    //}
}
