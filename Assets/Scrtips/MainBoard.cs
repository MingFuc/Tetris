using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainBoard : MonoBehaviour
{
    public static MainBoard instance;

    [SerializeField]
    private GameObject[] spawnBlocksArray;

    [SerializeField]
    private GameObject[] spawnGhostBlocksArray;

    private GameObject spawned_Block;
    [SerializeField]
    private GameObject spawned_GhostBlock;
    private GameObject incoming_Spawned_Block;
    [SerializeField]
    private GameObject incoming_Spawned_Ghost_Block;
    private GameObject image_Of_Incoming_Spawned_Block;

    [SerializeField]
    private GameObject ImageOfDestroyedBlock;

    [SerializeField]
    private GameObject replayArea;

    public Sprite[] colorSpawn = new Sprite[7];

    private Vector3 spawnPosition = new Vector3(6, 20);

    [SerializeField]
    private float[] spawnRotation = new float[4] { 0, 90, 180, 270 };
    public int topBoundary { get; private set; } = 22;

    public int[,] grid = new int[14, 22 + 2];

    public bool clearLineAndSpawn = false;

    public int ghostMoveUpRange = 0;

    public bool leftBlocked = false;
    public bool rightBlocked = false;

    public bool isGameOver = false;

    public TextMeshProUGUI tmp1;
    public TextMeshProUGUI tmp2;
    public float fallTime { get; private set; } = 0.5f;
    private int level = 1;
    private int scores = 0;

    private float time = 0;




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
        tmp1.text = scores.ToString();
        tmp2.text = "Level: " + level.ToString();
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

        RandomSpawn(true);
        IncomingSpawn();
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time > 30)
        {
            fallTime -= fallTime / 5; //level up
            level++;
            time = 0;
        }


        if (clearLineAndSpawn == true && isGameOver == false)
        {
            onDestroyGhostBlock_Invoke();
            ClearLine();
            tmp1.text = scores.ToString();
            tmp2.text = "Level: " + level.ToString();
            RandomSpawn(false);
            IncomingSpawn();
            clearLineAndSpawn = false;
        }
    }

    private void LateUpdate()
    {
        if (isGameOver)
        {
            replayArea.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                //reload scene here
                SceneManager.LoadScene("Tetris", LoadSceneMode.Single);
            }
        }
    }

    public void RandomSpawn(bool startGameSpawn)
    {
        if (startGameSpawn == true)
        {
            int i = UnityEngine.Random.Range(0, 7);
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
            spawned_Block = Instantiate(spawnBlocksArray[i], spawnPosition + offset_1 + offset_2, Quaternion.Euler(0, 0, spawnRotation[j]));
            spawned_GhostBlock = Instantiate(spawnGhostBlocksArray[i], spawnPosition + offset_1 + offset_2, Quaternion.Euler(0, 0, spawnRotation[j]));

        }
        else
        {
            incoming_Spawned_Block.SetActive(true);
            spawned_Block = incoming_Spawned_Block;

            incoming_Spawned_Ghost_Block.SetActive(true);
            spawned_GhostBlock = incoming_Spawned_Ghost_Block;
        }
    }

    public void IncomingSpawn()
    {
        int i = UnityEngine.Random.Range(0, 7);
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
        Vector3 offse_3 = (i == 2) ? new Vector3(-0.5f, 0.5f) : ((i == 3) ? new Vector3(-0.5f, 0) : new Vector3(0, 0)); // make the block in the middle of prediction part
        //set up incoming piece
        incoming_Spawned_Block = Instantiate(spawnBlocksArray[i], spawnPosition + offset_1 + offset_2, Quaternion.Euler(0, 0, spawnRotation[j]));

        int colorIndex = UnityEngine.Random.Range(0, 7);
        for (int ii = 0; ii < 4; ii++)
        {
            incoming_Spawned_Block.transform.GetChild(ii).GetComponent<SpriteRenderer>().sprite = MainBoard.instance.colorSpawn[colorIndex]; //set random color
        }

        if (image_Of_Incoming_Spawned_Block != null)
        {
            Destroy(image_Of_Incoming_Spawned_Block.gameObject);
        }

        image_Of_Incoming_Spawned_Block = Instantiate(incoming_Spawned_Block, new Vector3(15, 17) + offset_1 + offse_3, Quaternion.identity);
        image_Of_Incoming_Spawned_Block.GetComponent<BlockBehavior>().enabled = false;
        incoming_Spawned_Block.SetActive(false);

        //ghost piece
        incoming_Spawned_Ghost_Block = Instantiate(spawnGhostBlocksArray[i], spawnPosition + offset_1 + offset_2, Quaternion.Euler(0, 0, spawnRotation[j]));
        incoming_Spawned_Ghost_Block.SetActive(false);
    }

    public void ClearLine()
    {
        int offset = 0;
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
                for (int i = 2; i < 12; i++)
                {
                    Instantiate(ImageOfDestroyedBlock, new Vector3(i, j + offset), Quaternion.identity);
                }
                offset++;


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
                    if (Mathf.RoundToInt(gob.transform.position.y) == j)
                    {
                        
                        Destroy(gob.gameObject);


                    }
                    else if (Mathf.RoundToInt(gob.transform.position.y) >= j + 1)
                    {

                        gob.transform.Translate(new Vector2(0, -1), Space.World);

                    }
                }



                scores += level;


                j--;
            }


            valueSum = 0;
        }


        offset = 0;
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
