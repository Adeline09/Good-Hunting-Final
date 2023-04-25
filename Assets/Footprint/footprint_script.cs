using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footprint_script : MonoBehaviour
{
    // Start is called before the first frame update
    public float disappearTime;
    void Start()
    {
        disappearTime = 100.0f;
        Destroy(this.gameObject, disappearTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
