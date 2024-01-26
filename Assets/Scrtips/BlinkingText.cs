using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    private bool isOn = true;

    private float time = 0;


    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BlinkText();
    }
    

    void BlinkText()
    {
        time += Time.deltaTime;
        if(time > 1)
        {
            isOn = !isOn;
            tmp.enabled = isOn;
            time = 0;
        }
    }
}
