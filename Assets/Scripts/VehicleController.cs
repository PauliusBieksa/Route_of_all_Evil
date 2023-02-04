using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float TopSpeed;

    public bool OverrideSuspensionParams;
    public float SuspensionRestDistance;
    public float SuspensionSpringStrength;
    public float SuspensionDamper;

    public bool OverrideSteeringParams;
    public float SteeringGripFactor;
    public float SteeringWheelMass;

    public WheelPhysics[] SteerableWheelsFront;
    public WheelPhysics[] SteerableWheelsRear;
    public float maxSteerAngle;

    public bool OverrideDriveParams;
    public AnimationCurve DrivePowerCurve;
    [Range(0, 1)]
    public float DriveRollingResistance;
    public float DriveMaxTorque;


    // Update is called once per frame
    void Update()
    {
        float steeringInput = Input.GetAxis("Horizontal");
        foreach(WheelPhysics wheel in SteerableWheelsFront)
        {
            wheel.transform.localRotation = Quaternion.Euler(0, maxSteerAngle * steeringInput, 0);
        }

        foreach (WheelPhysics wheel in SteerableWheelsRear)
        {
            wheel.transform.localRotation = Quaternion.Euler(0, -maxSteerAngle * steeringInput, 0);
        }


        Debug.DrawRay(transform.position, GetComponent<Rigidbody>().mass * Physics.gravity, Color.white);
    }
}
