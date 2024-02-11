using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{


    //float screenHeight = 30;
    float screenWidth = 18.5f;
  
    void Start()
    {

        float orthoSize = screenWidth * ((float)Screen.height / (float)Screen.width) * 0.5f;

        Camera.main.orthographicSize = orthoSize;
    }

   
    
}
