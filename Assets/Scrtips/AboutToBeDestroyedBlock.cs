using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutToBeDestroyedBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(DestroyLineEffect());
    }

    IEnumerator DestroyLineEffect()
    {
        yield return new WaitForSeconds(.25f);

        
        Destroy(gameObject);
    }
}
