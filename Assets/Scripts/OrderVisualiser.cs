using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderVisualiser : MonoBehaviour
{
    public Transform targetObject;
    public VehicleController vehicle;
    public GameObject ArrowSystem;

    public TextMeshProUGUI Item;
    public TextMeshProUGUI Address;
    public TextMeshProUGUI Payout;

    private DirectionVisualiser arrow;

    private void Start()
    {
        arrow = Instantiate(ArrowSystem).GetComponentInChildren<DirectionVisualiser>();
        arrow.targetObject = targetObject;
        arrow.vehicle = vehicle;

    }

}
