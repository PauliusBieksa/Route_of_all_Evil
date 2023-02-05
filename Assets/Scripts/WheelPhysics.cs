using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPhysics : MonoBehaviour
{
    public float groundContactRayLength;
    public VehicleController vehicle;
    public float RestDistance;
    public float SpringStrength;
    public float SpringDamper;

    public float SteeringGrip;
    public float WheelMass;

    public AnimationCurve PowerCurve;
    [Range(0,1)]
    public float RollingResistance;
    public float MaxTorque;

    private Vector3 overallForce;
    private Rigidbody vehicleRigidbody;

    private void Start()
    {
        vehicleRigidbody = vehicle.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        CheckOverrides();

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 2;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundContactRayLength))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * groundContactRayLength, Color.white);
        
            CalculateSuspension(hit.distance);
            CalculateSteeringForce();
            CalculateDrive();
            ApplyForce();
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * groundContactRayLength, Color.red);
        }
    }

    private void CheckOverrides()
    {
        if (vehicle.OverrideSuspensionParams)
        {
            RestDistance = vehicle.SuspensionRestDistance;
            SpringStrength = vehicle.SuspensionSpringStrength;
            SpringDamper = vehicle.SuspensionDamper;
        }

        if(vehicle.OverrideSteeringParams)
        {
            SteeringGrip = vehicle.SteeringGripFactor;
            WheelMass = vehicle.SteeringWheelMass;
        }
        
        if(vehicle.OverrideDriveParams)
        {
            PowerCurve = vehicle.DrivePowerCurve;
            RollingResistance = vehicle.DriveRollingResistance;
            MaxTorque = vehicle.DriveMaxTorque;
        }
    }


    private void CalculateSuspension(float rayHitDistance)
    {
        overallForce = new Vector3(0, 0, 0);
        //world-space direction of the upwards spring force
        Vector3 springDirection = transform.up;

        Vector3 wheelWorldVelocity = vehicleRigidbody.GetPointVelocity(transform.position);
        float offset = RestDistance - rayHitDistance;

        //project the magnitude of the velocity onto the vertical axis
        float suspensionTravelVelocity = Vector3.Dot(springDirection, wheelWorldVelocity);

        //calualte magnitude of the damped spring force
        float force = (offset * SpringStrength) - (suspensionTravelVelocity * SpringDamper);

        Vector3 suspensionForce = springDirection * force;

        Debug.DrawRay(transform.position, suspensionForce, Color.green);
        overallForce += suspensionForce;
    }

    
    private void CalculateSteeringForce()
    {
        Vector3 steeringDirection = transform.right;

        Vector3 wheelWorldVelocity = vehicleRigidbody.GetPointVelocity(transform.position);

        float  steeringVelocity = Vector3.Dot(steeringDirection, wheelWorldVelocity);

        float desiredVelocityChange = -steeringVelocity * SteeringGrip;

        float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

        Vector3 steeringForce = (steeringDirection * WheelMass * desiredAcceleration);
        Debug.DrawRay(transform.position, steeringForce, Color.red);
        overallForce += steeringForce;

    }

    private void CalculateDrive()
    {
        Vector3 accelerationDirection = transform.forward;

        float accelerationInput = Input.GetAxis("Vertical");

        float vehicleSpeed = Vector3.Dot(vehicle.transform.forward, vehicleRigidbody.velocity);
        //Speed as a normalised percentage of max.
        float normalisedSpeed = Mathf.Clamp01(Mathf.Abs(vehicleSpeed) / vehicle.TopSpeed);
        Vector3 driveForce = new Vector3(0,0,0);



        //Forward
        if (accelerationInput != 0 && vehicleSpeed < vehicle.TopSpeed)
        {
            //Available power for this wheel
            float availableTorque = PowerCurve.Evaluate(normalisedSpeed) * MaxTorque * accelerationInput;
            driveForce = accelerationDirection * availableTorque;
            overallForce += driveForce;
        }


        //Neutral;
        if (accelerationInput == 0 && vehicleSpeed > -vehicle.TopSpeed)
        {
            Vector3 deccelerationDirection = -transform.forward;
            driveForce = deccelerationDirection * vehicleSpeed * RollingResistance;
            overallForce += driveForce;
            if (vehicleSpeed < 0.1 && vehicleSpeed > -0.1)
            {
                vehicleRigidbody.velocity = new Vector3(0, 0, 0);
            }
        }

        Debug.DrawRay(transform.position, driveForce, Color.blue);

    }

    private void ApplyForce()
    {
        Debug.DrawRay(transform.position, overallForce, Color.yellow);
        vehicleRigidbody.AddForceAtPosition(overallForce, transform.position);
    }

}
