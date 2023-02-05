using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionVisualiser : MonoBehaviour
{
    public Transform targetObject;
    public VehicleController vehicle;
    public Camera RenderCam;
    public RenderTexture TextureFormat;
    public RawImage displayImage;

    public static int NextPosition;

    private RenderTexture targetTex;

    private void Start()
    {
        targetTex = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        targetTex.descriptor = TextureFormat.descriptor;
        targetTex.Create();
        RenderCam.targetTexture = targetTex;
        displayImage.texture = targetTex;

        
        Transform parent = GetComponentsInParent<Transform>()[1];
        parent.position = new Vector3(NextPosition, parent.position.y, parent.position.z);
        NextPosition += 10;

        //vehicle = GameObject.Find("Van").GetComponent<VehicleController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = targetObject.position - vehicle.transform.position;
        transform.LookAt(new Vector3(offset.x, transform.position.y, offset.z));
        transform.Rotate(0, vehicle.transform.rotation.y, 0);
    }
}
