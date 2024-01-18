using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    [SerializeField]
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
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                transform.Rotate(0, 0, 90);
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
            MainBoard.instance.RandomSpawn();
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
            if (transform.GetChild(i).position.x < -0.001)
            {
                MoveRight();
                break;
            }
            if (transform.GetChild(i).position.x > 9.001)
            {
                MoveLeft();
                break;
            }
        }
        //check collide with ground
        /*for (int i = 0; i < 4; i++)
        {
            if (transform.GetChild(i).position.y < 1 - 0.001)
            {
                AddGrid();
                isCollide = true;
                continueSpawn = true;
                break;
            }
        }*/
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
            Debug.Log("add" + Mathf.RoundToInt(transform.GetChild(i).position.x) + "," + Mathf.RoundToInt(transform.GetChild(i).position.y));
        }
    }
}
