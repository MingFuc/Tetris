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
    private GameObject[] spawnBlock;

    private Vector3 spawnPosition = new Vector3(4, 17);

    [SerializeField]
    private float[] spawnRotation = new float[4] { 0, 90, 180, 270 };

    public static int[,] grid = new int[14, 22];

    public static bool clearLineAndSpawn = false;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int j = 2; j < 22; j++)
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

        RandomSpawn();
    }
    private void Update()
    {
        if(clearLineAndSpawn == true)
        {
            ClearLine();
            RandomSpawn();
            clearLineAndSpawn = false;
        }
    }

    public void RandomSpawn()
    {
        //int i = UnityEngine.Random.Range(0, 7);
        int i = 0;
        int j = UnityEngine.Random.Range(0, 4);
        Vector3 offset = (i == 0 || i == 3) ? new Vector2(0.5f, 0.5f) : new Vector2(0, 0);
        Instantiate(spawnBlock[i], spawnPosition + offset, Quaternion.Euler(0, 0, spawnRotation[j]));
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
                //grid affects
                for (int i = 2; i < 12; i++)
                {
                    grid[i, j] = 0;

                    int a = 1;
                    do
                    {
                        if (grid[i, j + a] == 1)
                        {
                            grid[i, j + a] = 0;
                            grid[i, j + a - 1] = 1;
                        }
                        a++;
                    }
                    while (j + a < 22);
                    a = 1;
                }
                //visual affects
                foreach (GameObject gob in GameObject.FindGameObjectsWithTag("Block"))
                {
                    if (gob.transform.position.y == j)
                    {
                        Destroy(gob.gameObject);
                    }
                    else if (gob.transform.position.y > j)
                    {
                        gob.transform.Translate(new Vector2(0, -1), Space.World);
                    }
                }

                j--;
            }
            valueSum = 0;
        }
    }
}
