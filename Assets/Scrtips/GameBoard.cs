using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameBoard : Singleton<GameBoard>
{

    [SerializeField]
    private GameObject[] gob;

    [SerializeField]
    private GameObject[] piece = new GameObject[4];

    private int[,] pieceID =
    {
        {1,3,5,7 }, //I
        {0,1,2,3 }, //O
        {0,2,3,5 }, //Z
        {1,2,3,4 }, //S
        {1,2,3,5 }, //T
        {0,2,4,5 }, //L
        {1,3,4,5 }  //P
    };

    [SerializeField]
    private float time = 0;

    private bool inactivePiece = true; //if this set to true it will spawn next piece
    private bool hitTheBlock = false;

    private int leftBound = 0;
    private int rightBound = 9;

    private float dropSpeed = 1f;

    public bool[,] grid = new bool[50, 50];
    private void Start()
    {
        Render(Random.Range(0, 7), piece, Random.Range(0, 7));
    }
    private void Update()
    {
        CheckCollideBlock();
        Render(Random.Range(0, 7), piece, Random.Range(0, 7));
        if (inactivePiece == false)
        {
            FallOff();
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(-1, 0);
                LeftBound();
                //prevent collide from rightside
                for (int i = 0; i < 4; i++)
                {
                    int xGridPosToCheck = (int)(piece[i].transform.position.x);
                    int yGridPosToCheck = (int)(piece[i].transform.position.y);
                    if (grid[Mathf.Abs(xGridPosToCheck), Mathf.Abs(yGridPosToCheck)] == true)
                    {
                        Move(1, 0);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Move(1, 0);
                RightBound();
                //prevent collide from leftside
                for (int i = 0; i < 4; i++)
                {
                    int xGridPosToCheck = (int)(piece[i].transform.position.x);
                    int yGridPosToCheck = (int)(piece[i].transform.position.y);
                    if (grid[Mathf.Abs(xGridPosToCheck), Mathf.Abs(yGridPosToCheck)] == true)
                    {
                        Move(-1, 0);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Rotate();
                LeftBound();
                RightBound();
            }
            if (Input.GetKeyDown(KeyCode.S))
                dropSpeed = 0.01f;

        }
    }
    void Render(int n, GameObject[] piece, int r)
    {
        if (inactivePiece == true)
        {
            for (int i = 0; i < 4; i++)
            {
                dropSpeed = 1f;
                piece[i] = Instantiate(gob[r], new Vector2(pieceID[n, i] % 2, -pieceID[n, i] / 2), gameObject.transform.rotation); //render each block for 1 piece
            }
            inactivePiece = false;
        }
    }
    void Move(int dx, int dy)
    {
        for (int i = 0; i < 4; i++)
        {
            piece[i].transform.Translate(dx, dy, 0);
        }
    }
    void Rotate() //need to optimize
    {
        for (int i = 0; i < 4; i++)
        {
            float x = -(piece[i].transform.position.y - piece[1].transform.position.y) + piece[1].transform.position.x;
            float y = (piece[i].transform.position.x - piece[1].transform.position.x) + piece[1].transform.position.y;
            piece[i].transform.position = new Vector2(x, y);
        }

    }
    void FallOff()
    {
        time += Time.deltaTime;
        if (time > dropSpeed && inactivePiece == false )
        {
            Move(0, -1);
            time = 0;
        }
    }
    void LeftBound()
    {
        bool outOfBoundLeft = false;
        float leftOffset = 0;
        for (int i = 0; i < 4; i++)
        {
            if (piece[i].transform.position.x < leftBound)
            {
                outOfBoundLeft = true;
                leftOffset = piece[i].transform.position.x;
            }
        }
        if (outOfBoundLeft == true)
        {
            for (int i = 0; i < 4; i++)
            {
                piece[i].transform.position += new Vector3(leftBound - leftOffset, 0, 0);
            }
            leftOffset = 0;
            outOfBoundLeft = false;
        }

    }
    void RightBound()
    {
        bool outOfBoundRight = false;
        float rightOffset = 0;
        for (int i = 0; i < 4; i++)
        {
            if (piece[i].transform.position.x > rightBound)
            {
                outOfBoundRight = true;
                rightOffset = piece[i].transform.position.x;
            }
        }
        if (outOfBoundRight == true)
        {
            for (int i = 0; i < 4; i++)
            {
                piece[i].transform.position += new Vector3(rightBound - rightOffset, 0, 0);
            }
            rightOffset = 0;
            outOfBoundRight = false;
        }
    }
    void AddGrid()
    {
        for (int i = 0; i < 4; i++)
        {
            int xGridPos = (int)(piece[i].transform.position.x);
            int yGridPos = (int)(piece[i].transform.position.y);
            grid[Mathf.Abs(xGridPos), Mathf.Abs(yGridPos)] = true;
            Debug.Log("add at " + xGridPos + yGridPos);
        }
    }
    void CheckCollideBlock()
    {
        //check if hit the ground
        for (int i = 0; i < 4; i++)
        {
            if (piece[i].transform.position.y == -19)
            {
                AddGrid();
                inactivePiece = true;
                return;
            }
        }
        //check if hit other piece
        for (int i = 0; i < 4; i++)
        {
            int xGridPosToCheck = (int)(piece[i].transform.position.x);
            int yGridPosToCheck = (int)(piece[i].transform.position.y);
            if (grid[Mathf.Abs(xGridPosToCheck), Mathf.Abs(yGridPosToCheck)] == true)
            {
                hitTheBlock = true;
            }
        }
        if (hitTheBlock == true)
        {
            Move(0, 1);
            AddGrid();
            hitTheBlock = false;
            inactivePiece = true;
        }
    }
}
