using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderVisualiser : MonoBehaviour
{
    
    public GameObject ArrowSystem;

    private VehicleController vehicle;
    public TextMeshProUGUI Item;
    public TextMeshProUGUI Address;
    public TextMeshProUGUI Payout;

    private DirectionVisualiser arrow;

    public void Initialise(GameState.Order order)
    {
        vehicle = GameObject.FindObjectOfType<VehicleController>();
        Item.text = order.item;
        Address.text = order.address;
        Payout.text = $"${order.reward:0.##}";

        arrow = Instantiate(ArrowSystem).GetComponentInChildren<DirectionVisualiser>();
        arrow.targetObject = order.building.transform;
        arrow.vehicle = vehicle;
    }
}
