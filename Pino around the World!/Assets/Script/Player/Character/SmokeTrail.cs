using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeTrail : MonoBehaviour
{
    PlayerMovement playerM;
    [SerializeField]
    ParticleSystem ps, ps1;

    [System.Obsolete]
    private void Start()
    {
        playerM = FindObjectOfType<PlayerMovement>();
        ps.enableEmission = false;
        ps1.enableEmission = false;
    }

    [System.Obsolete]
    private void Update()
    {
        if (playerM.barbarianRun)
        {
            ps.enableEmission = true;
            ps1.enableEmission = true;
        }
        else
        {
            Emission();
        }
    }

    [System.Obsolete]
    void Emission()
    {
        ps.enableEmission = false;
        ps1.enableEmission = false;
    }
}
