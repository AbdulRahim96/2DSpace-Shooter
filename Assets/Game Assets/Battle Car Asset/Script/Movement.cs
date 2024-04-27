using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public Transform[] wheelsTransforms;
    Vector3 pos, rotation;
    Quaternion quat;
    public float force, rotationSpeed;


    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.eulerAngles = rotation;

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].GetWorldPose(out pos, out quat);
            wheelsTransforms[i].position = pos;
            wheelsTransforms[i].rotation = quat;
        }

        foreach (var wColliders in wheelColliders)
        {
            wColliders.motorTorque = Input.GetAxis("Vertical") * force * Time.deltaTime;
        }

        rotation.y += Input.GetAxis("Horizontal") * rotationSpeed;
    }
}
