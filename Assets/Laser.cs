using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : BulletParticleSystem
{
    [Range(1, 5)] public float duration = 5;
    private float durtationTime;
    private bool isPlaying = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        currenTime -= Time.deltaTime;
        if(currenTime <= 0)
        {
            if(!isPlaying)
            {
                bulletParticle.Play();
                durtationTime = duration;
                isPlaying = true;
            }

            durtationTime -= Time.deltaTime;
            if(durtationTime <= 0)
            {
                currenTime = timeGap;
                isPlaying = false;
                bulletParticle.Stop();
            }

        }
    }
}
