using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameBoard : MonoBehaviour
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
        {1,3,4,5 }  //J
    };

    [SerializeField]
    private float time = 0;

    private bool inactivePiece = true; //if this set to true it will spawn next piece
    //private bool hitTheBlock = false;

    private int leftBound = 0;
    private int rightBound = 9;

    private float dropSpeed = 1f;

    public bool[,] grid = new bool[10, 20];
    private void Start()
    {
        Render(0, piece, Random.Range(0, 7));
        //Render(Random.Range(0, 7), piece, Random.Range(0, 7));
        //set all value of grid to false
        for (int j = 0; j < 20; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                grid[i, j] = false;
            }
        }
    }
    private void Update()
    {
        CheckCollideBlock();
        ClearLine();
        Render(0, piece, Random.Range(0, 7));
        //Render(Random.Range(0, 7), piece, Random.Range(0, 7));
        FallOff();
        if (inactivePiece == false)
        {
            //FallOff();
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
            {
                dropSpeed = 0.05f;
                //Move(0, -1);
            }

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
        if (time > dropSpeed && inactivePiece == false)
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
            grid[Mathf.Abs((int)(piece[i].transform.position.x)), Mathf.Abs((int)(piece[i].transform.position.y))] = true;
            //Debug.Log("add at " + xGridPos + yGridPos);
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
            else
                inactivePiece = false;
        }
        //check if hit other piece
        for (int i = 0; i < 4; i++)
        {
            int xGridPosToCheck = (int)(piece[i].transform.position.x);
            int yGridPosToCheck = (int)(piece[i].transform.position.y);
            if (grid[Mathf.Abs(xGridPosToCheck), Mathf.Abs(yGridPosToCheck)] == true)
            {
                Move(0, 1);
                AddGrid();
                inactivePiece = true;
                return;
            }
            else
                inactivePiece = false;
        }
    }
    void ClearLine()
    {
        for (int j = 0; j < 20; j++)
        {
            int countBlock = 0;
            for (int i = 0; i < 10; i++)
            {
                if (grid[i, j] == true)
                {
                    countBlock++;
                }
            }

            if (countBlock >= 10)
            {
                Debug.Log("thoa man dong " + j);
                //destroy line and move down line 
                foreach (GameObject gob in GameObject.FindGameObjectsWithTag("Block"))
                {
                    if (gob.transform.position.y == -j)
                    {
                        grid[(int)gob.transform.position.x, j] = false;
                        Destroy(gob.gameObject);
                        //Debug.Log("destroy line " + j);
                    }
                    else if (gob.transform.position.y > -j-0.001)
                    {
                        grid[(int)gob.transform.position.x, Mathf.Abs((int)gob.transform.position.y)] = false;
                        gob.transform.position += new Vector3(0, -1, 0);
                        grid[(int)gob.transform.position.x, Mathf.Abs((int)gob.transform.position.y)] = true;
                    }
                    else
                    {
                        grid[(int)gob.transform.position.x, Mathf.Abs((int)gob.transform.position.y)] = true;
                    }
                }

                j--; //recheck the loop in case multiple consecutive lines need to be deleted
            }
            countBlock = 0;
        }
    }
}
