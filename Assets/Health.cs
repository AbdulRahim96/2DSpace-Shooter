using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public bool isPlayer = false;
    public int coinsEarned = 100;
    public float maxDamage, healthRegainRate = 5;
    public Slider healthBar;
    public GameObject destroyEffect;
    public ParticleSystem hitParticle;

    public int AI_Health = 3;

    public UnityEvent AfterDestroy;
    private void FixedUpdate()
    {
        if (!healthBar) return;

        if (healthBar.value < maxDamage)
            healthBar.value += Time.deltaTime * healthRegainRate;

    }
    public void HealthUpdate(float damage)
    {
        if(!healthBar)
        {
            AI_Health--;
            if (AI_Health <= 0)
                DestroyOBJ();
        }
        else
        {
            healthBar.value -= damage;
            if (healthBar.value <= 0)
            {
                DestroyOBJ();
            }
        }
          
        if(hitParticle)
            hitParticle.Play();
    }

    public void DestroyOBJ(float time = 0.2f)
    {
        AfterDestroy.Invoke();
        GameObject fx = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(fx, 3);
        Destroy(gameObject, time);
    }
}
