using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBomb : BulletParticleSystem
{

    public float speed = 5;
    private void Start()
    {
        Destroy(gameObject, timeGap);
    }
    private void FixedUpdate()
    {
        transform.localScale += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if(collision.gameObject.TryGetComponent<Health>(out Health h))
            h.HealthUpdate(damage);
    }
   
}
