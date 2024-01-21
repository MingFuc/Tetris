using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class MainBoard : MonoBehaviour
{
    public static MainBoard instance;

    [SerializeField]
    private GameObject[] spawnBlocks;

    [SerializeField]
    private GameObject[] spawnGhostBlocks;

    private GameObject spawned_Block;
    private GameObject spawned_GhostBlock;

    private Vector3 spawnPosition = new Vector3(6, 20);

    [SerializeField]
    private float[] spawnRotation = new float[4] { 0, 90, 180, 270 };
    public int topBoundary { get; private set; } = 21;

    public static int[,] grid = new int[14, 22 + 2];

    public static bool clearLineAndSpawn = false;

    public static int ghostMoveUpRange = 0;

    public static bool leftBlocked = false;
    public static bool rightBlocked = false;

    public event Action onDestroyGhostBlock;
    public void onDestroyGhostBlock_Invoke()
    {
        if (onDestroyGhostBlock != null)
        {
            onDestroyGhostBlock.Invoke();
        }
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //set up grid
        for (int j = 2; j < 22 + 2; j++)
        {
            for (int i = 2; i < 12; i++)
            {
                grid[i, j] = 0;  //inside grid set to 0
            }
        }
        for (int i = 0; i < 12; i++)
        {
            grid[i, 0] = 1; //set value to ground
            grid[i, 1] = 1;
        }
        //

        RandomSpawn();
    }
    private void Update()
    {
        if (clearLineAndSpawn == true)
        {
            onDestroyGhostBlock_Invoke();
            ClearLine();
            RandomSpawn();
            clearLineAndSpawn = false;
        }
    }

    public void RandomSpawn()
    {
        int i = UnityEngine.Random.Range(0, 7);
        //int i = 0;
        int j = UnityEngine.Random.Range(0, 4);
        Vector3 offset_1 = (i == 0 || i == 3) ? new Vector2(0.5f, 0.5f) : new Vector2(0, 0); //make block fit in grid (case I and O blocks)
        Vector3 offset_2 = new Vector2();                                                    //make block touch the topbound
        if (i == 0)                                         //I
        {
            if (j == 0 || j == 2)
                offset_2 = new Vector2(0, -1);
            else if (j == 1)
                offset_2 = new Vector2(0, 1);
            else
                offset_2 = new Vector2(0, 0);
        }
        else if (i == 1 || i == 4 || i == 5 || i == 6)      //J,S,T,Z
        {
            if (j == 2)
                offset_2 = new Vector2(0, 1);
            else
                offset_2 = new Vector2(0, 0);
        }
        else if (i == 2)                                     //L
        {
            if (j == 3)
                offset_2 = new Vector2(0, 1);
            else
                offset_2 = new Vector2(0, 0);
        }
        else                                                 //O
            offset_2 = new Vector2(0, 0);
        spawned_Block = Instantiate(spawnBlocks[i], spawnPosition + offset_1 + offset_2, Quaternion.Euler(0, 0, spawnRotation[j]));
        spawned_GhostBlock = Instantiate(spawnGhostBlocks[i], spawnPosition + offset_1 + offset_2, Quaternion.Euler(0, 0, spawnRotation[j]));
    }
    public void ClearLine()
    {
        for (int j = 2; j < 22; j++)
        {
            //loop per line
            int valueSum = 0;
            for (int i = 2; i < 12; i++)
            {
                valueSum += grid[i, j];
            }
            if (valueSum >= 10) //if line is full
            {
                Debug.Log($"=================> Line j = {j}");
                //grid affects
                for (int i = 2; i < 12; i++)
                {
                    grid[i, j] = 0;
                    Debug.Log($"[{i},{j}] = 0");

                    int a = 1;
                    do
                    {
                        if (grid[i, j + a] == 1)
                        {
                            grid[i, j + a] = 0;
                            Debug.Log($"[{i},{j + a}] = 0");
                            grid[i, j + a - 1] = 1;
                            Debug.Log($"[{i},{j + a - 1}] = 1");
                        }
                        a++;
                    }
                    while (j + a < 22);
                    a = 1;
                }
                //visual affects
                foreach (GameObject gob in GameObject.FindGameObjectsWithTag("Block"))
                {
                    if (Mathf.RoundToInt(gob.transform.position.y) == j)
                    {
                        Debug.Log($"{gob.transform.position.x},{gob.transform.position.y} is destroyed");
                        Destroy(gob.gameObject);
                    }
                    else if (Mathf.RoundToInt(gob.transform.position.y) >= j + 1)
                    {
                        Debug.Log($"{gob.transform.position.x},{gob.transform.position.y} moved to");
                        gob.transform.Translate(new Vector2(0, -1), Space.World);
                        Debug.Log($"{gob.transform.position.x},{gob.transform.position.y}");
                    }
                }

                j--;
            }
            valueSum = 0;
        }
    }
    public bool CheckVertically() //between ghost block and normal block,  have a block ? ==> ghost block move up (like it should) 
    {
        for (int i = 0; i < 4; i++)
        {
            int block_yPos = Mathf.RoundToInt(spawned_Block.transform.GetChild(i).position.y);
            int ghostBlock_yPos = Mathf.RoundToInt(spawned_GhostBlock.transform.GetChild(i).position.y);

            int stepNumber = 1;
            do
            {
                if (grid[Mathf.RoundToInt(spawned_GhostBlock.transform.GetChild(i).position.x),
                    ghostBlock_yPos + Mathf.RoundToInt(stepNumber)] == 1)
                {
                    ghostMoveUpRange = stepNumber;
                    return true;
                }
                stepNumber++;
            }
            while (ghostBlock_yPos + stepNumber < block_yPos);
        }
        return false;
    }
}
