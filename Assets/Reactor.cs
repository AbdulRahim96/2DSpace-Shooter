using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour
{
    public Vector3 offset;
    public Transform attachedObject;
    public GameObject finishPoint;
    private bool canAttach = true;
    public void AttatchObject(Transform other)
    {
        if (!canAttach) return;
        other.SetParent(transform);
        other.localPosition = Vector3.zero + offset;
        attachedObject = other;
        finishPoint.SetActive(true);
    }

    public async void DetachObject()
    {
        attachedObject.parent = null;
        attachedObject = null;
        finishPoint.SetActive(false);
        canAttach = false;
        await PlayerControls.WaitAsync(1);
        canAttach = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collide");
        if (attachedObject)
            DetachObject();
    }
}
