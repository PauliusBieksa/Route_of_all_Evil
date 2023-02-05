using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpedometerNeedle : MonoBehaviour
{
    public Image Needle;
    public float maxRotation;
    public float startRotation;
    public VehicleController vehicle;



    // Update is called once per frame
    void Update()
    {
        float proportionalSpeed = vehicle.GetComponent<Rigidbody>().velocity.magnitude / vehicle.TopSpeed;
        float angle = Mathf.Lerp(startRotation, maxRotation, proportionalSpeed);

        var eulerAngles = transform.eulerAngles;
        Needle.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, angle);
    }
}
