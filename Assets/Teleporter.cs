using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform otherSide;
    public float pushForce = 10;
    public bool canTeleport = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!canTeleport) return;
        otherSide.GetComponent<Teleporter>().canTeleport = false;
        other.transform.position = otherSide.position;
        other.GetComponent<Rigidbody>().AddForce(other.transform.forward * pushForce, ForceMode.Impulse);
    }
    private void OnTriggerExit(Collider other)
    {
        canTeleport = true;
    }
}
