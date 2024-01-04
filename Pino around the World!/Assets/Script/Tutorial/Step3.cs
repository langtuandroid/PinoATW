using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step3 : MonoBehaviour
{
    Tutorial tutorial;

    private void Awake()
    {
        tutorial = GetComponentInParent<Tutorial>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            tutorial.pause = true;
            tutorial.step3 = true;
        }
    }
}
