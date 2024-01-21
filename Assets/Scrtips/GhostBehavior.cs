using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    private float time = 0;

    private float fallTime = 0.005f;

    private bool isCollide = false;

    private void Awake()
    {
        MainBoard.instance.onDestroyGhostBlock += DestroyThis;
    }

    private void OnDestroy()
    {
        MainBoard.instance.onDestroyGhostBlock -= DestroyThis;
    }
    private void Update()
    {
        //sync norm and ghost
        if (MainBoard.leftBlocked == true)
        {
            MoveRight();
            MainBoard.leftBlocked = false;
        }
        if (MainBoard.rightBlocked == true)
        {
            MoveLeft();
            MainBoard.rightBlocked = false;
        }
        //

        if (isCollide == false)
        {
            FallDown();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            isCollide = false;
            MoveLeft();

            while (MainBoard.instance.CheckVertically() == true)
            {
                gameObject.transform.Translate(new Vector2(0, MainBoard.ghostMoveUpRange), Space.World);
            }
            //prevent collide from rightside
            //for (int i = 0; i < 4; i++)
            //{
            //    if (MainBoard.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
            //    {
            //        MoveRight();
            //    }
            //}
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            isCollide = false;
            MoveRight();

            //MainBoard.instance.CheckHorizontally();
            while (MainBoard.instance.CheckVertically() == true)
            {
                gameObject.transform.Translate(new Vector2(0, MainBoard.ghostMoveUpRange), Space.World);
            }
            //prevent collide from leftside
            //for (int i = 0; i < 4; i++)
            //{
            //    if (MainBoard.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
            //    {
            //        MoveLeft();
            //    }
            //}
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            isCollide = false;
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
        CheckBound();
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
        //check grid below
        for (int i = 0; i < 4; i++)
        {
            if (MainBoard.grid[Mathf.RoundToInt(transform.GetChild(i).position.x), Mathf.RoundToInt(transform.GetChild(i).position.y)] == 1)
            {
                MoveUp();
                isCollide = true;
                //continueSpawn = true;
                break;
            }
        }
    }
    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
