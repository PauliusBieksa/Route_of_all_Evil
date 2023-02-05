using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderVisualiser : MonoBehaviour
{
    public VehicleController vehicle;
    public GameObject ArrowSystem;

    public TextMeshProUGUI Item;
    public TextMeshProUGUI Address;
    public TextMeshProUGUI Payout;

    private DirectionVisualiser arrow;

    public void Initialise(GameState.Order order)
    {
        Item.text = order.item;
        Address.text = order.address;
        Payout.text = $"${order.reward:0.##}";

        arrow = Instantiate(ArrowSystem).GetComponentInChildren<DirectionVisualiser>();
        arrow.targetObject = order.building.transform;
        arrow.vehicle = vehicle;
    }
}
