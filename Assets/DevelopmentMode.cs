using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevelopmentMode : MonoBehaviour
{
    public PlayerControls controls;
    public Transform cam;
    public Text speed, smoothness, boostSpeed, rechargeTime, wormhole, camAngle;

    private void Awake()
    {
        Hole.pullStrength = 200;
    }
    public void adjustSpeed(float val)
    {
        controls.moveSpeed = val;
        speed.text = "speed: " + val.ToString("0.0");
    }

    public void adjustsmoothness(float val)
    {
        controls.smoothness = val;
        smoothness.text = "brakes: " + val.ToString("0.00");
    }

    public void adjustboostSpeed(float val)
    {
        controls.BoostStrength = val;
        boostSpeed.text = "Boost strenght: " + val.ToString("0");
    }

    public void adjustrecharge(float val)
    {
        controls.BoostRechargeTime = val;
        rechargeTime.text = "Boost recharge: " + val.ToString("0.0") + "s";
    }

    public void warmholeStrengh(float val)
    {
        Hole.pullStrength = val;
        wormhole.text = "worm hole force: " + val.ToString("0") + "N";
    }

    public void angle(float val)
    {
        cam.localEulerAngles = new Vector3(val, 0, 0);
        camAngle.text = "Camera Angle: " + val.ToString("0.00") + "'";
    }
}
