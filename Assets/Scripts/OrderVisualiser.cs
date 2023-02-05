using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderVisualiser : MonoBehaviour
{

    public static Action<GameState.Order> OnOrderRemove;

    public GameObject ArrowSystem;

    private VehicleController vehicle;
    public TextMeshProUGUI Item;
    public TextMeshProUGUI Address;
    public TextMeshProUGUI Payout;
    public RawImage arrowImage;
    private GameState.Order thisOrder;
    

    private DirectionVisualiser arrow;

    private void OnEnable()
    {
        OnOrderRemove += Destructor;
    }

    public void Initialise(GameState.Order order, int orderNum)
    {
        thisOrder = order;
        vehicle = GameObject.FindObjectOfType<VehicleController>();
        Transform[] vehicleArrowHolders = vehicle.transform.Find("ArrowOffsets").GetComponentsInChildren<Transform>();
        Item.text = order.item;
        Address.text = order.address;
        Payout.text = $"${order.reward:0.##}";

        arrow = Instantiate(ArrowSystem).GetComponentInChildren<DirectionVisualiser>();
        arrow.targetObject = order.building.transform;
        arrow.VehicleArrowTransform = vehicleArrowHolders[orderNum+1];
        arrow.displayImage = arrowImage;
    }

    private void Destructor(GameState.Order order)
    {
        if (order == thisOrder)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        OnOrderRemove += Destructor;
    }
}
