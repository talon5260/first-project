using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabbyChange : MonoBehaviour
{
    Animator amt;
    int val;
   

    // Update is called once per frame
    void Awake()
    {
        amt = GetComponent<Animator>();
    }

    void Update()
    {
        val = Random.Range(-5, 5);
        if (val >= 0)
        {
            amt.SetBool("change", true);
        }
        else if (val < 0)
        {
            amt.SetBool("change", false);
        }
    }
}
