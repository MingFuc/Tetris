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

    public static int[,] grid = new int[14, 21];

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int j = 1; j < 21; j++)
        {
            for (int i = 2; i < 12; i++)
            {
                grid[i, j] = 0;  //inside grid set to 0
            }
        }
        for (int i = 0; i < 12; i++)
        {
            grid[i, 0] = 1; //set value to ground
        }
         
        RandomSpawn();
    }

    public void RandomSpawn()
    {
        int i = UnityEngine.Random.Range(0, 7);
        int j = UnityEngine.Random.Range(0, 4);
        Vector3 offset = (i == 0 || i == 3) ? new Vector2(0.5f, 0.5f) : new Vector2(0, 0);
        Instantiate(spawnBlock[i], spawnPosition + offset, Quaternion.Euler(0, 0, spawnRotation[j]));
    }
}
