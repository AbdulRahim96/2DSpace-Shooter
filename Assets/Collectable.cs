using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    public GameObject effect;
    public UnityEvent triggerEvent;
    public bool destroyAfterTrigger = true;
    [Header("Floatable")]
    public bool isFloating = false;
    public float forceSpeed = 5;
    private void Start()
    {
        if (isFloating)
            GetComponent<Rigidbody>().AddForce(transform.forward * forceSpeed, ForceMode.Impulse);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            triggerEvent.Invoke();
            if(effect)
            {
                GameObject obj = Instantiate(effect, transform.position, transform.rotation);
                Destroy(obj, 3);
            }

            if (destroyAfterTrigger)
                Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isFloating) return;
        // Get the contact point of the collision
        ContactPoint contact = collision.contacts[0];

        // Calculate the reflection direction
        Vector3 reflectDirection = Vector3.Reflect(transform.forward, contact.normal).normalized;

        // Apply force in the opposite direction from the hit point
        GetComponent<Rigidbody>().AddForce(reflectDirection * forceSpeed, ForceMode.Impulse);
    }
}
