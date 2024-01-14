using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject gob;
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
    private void Start()
    {
        Render(Random.Range(0,7));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Move(-1);
        if (Input.GetKeyDown(KeyCode.D))
            Move(1);
    }
    void Render(int n)
    {
        for (int i = 0; i < 4; i++)
        {
            piece[i] = Instantiate(gob, new Vector2(pieceID[n,i] / 2, pieceID[n,i] % 2), gameObject.transform.rotation ); //render each block for 1 piece
        }
    }
    void Move(int dx)
    {
        for (int i = 0; i < 4; i++)
        {
            piece[i].transform.Translate(dx,0,0); 
        }
    }
}
