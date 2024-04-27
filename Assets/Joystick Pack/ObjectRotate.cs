using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectRotate : MonoBehaviour
{
    public enum axisType
    {
        x_axis,
        y_axis,
        z_axis,
    }
    public axisType axis;
    public float rotationSpeed = 5;
    void FixedUpdate()
    {
        switch (axis)
        {
            case axisType.x_axis: transform.Rotate(rotationSpeed*Time.deltaTime, 0, 0); break;
            case axisType.y_axis: transform.Rotate(0, rotationSpeed* Time.deltaTime, 0); break;
            case axisType.z_axis: transform.Rotate(0, 0, rotationSpeed* Time.deltaTime); break;
        }
    }

    public void restart()
    {
        int lev = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(lev);
    }
}
