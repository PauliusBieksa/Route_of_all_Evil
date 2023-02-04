using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpedometerNeedle : MonoBehaviour
{
    public Image Needle;
    public Vector3 maxRotationInput;
    public VehicleController vehicle;


    private Quaternion startRot;
    private Quaternion maxRot;

    // Update is called once per frame
    void Update()
    {
        float proportionalSpeed = vehicle.GetComponent<Rigidbody>().velocity.magnitude / vehicle.TopSpeed;

        maxRot = Quaternion.Euler(maxRotationInput);
        Quaternion.Slerp(startRot, maxRot, proportionalSpeed);
    }
}
