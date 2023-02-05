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
    private int thisOrderIndex;
    private GameState gameState;
    private Vector3 startScale;
    private Vector3 highlightScale;
    

    private DirectionVisualiser arrow;
    

    public void Initialise(GameState.Order order, int orderNum)
    {
        OnOrderRemove += Destructor;
        gameState = GameObject.FindObjectOfType<GameState>();
        thisOrder = order;
        thisOrderIndex = orderNum;
        vehicle = GameObject.FindObjectOfType<VehicleController>();
        Transform[] vehicleArrowHolders = vehicle.transform.Find("ArrowOffsets").GetComponentsInChildren<Transform>();
        Item.text = order.item;
        Address.text = order.address;
        Payout.text = $"${order.reward:0.##}";

        arrow = Instantiate(ArrowSystem).GetComponentInChildren<DirectionVisualiser>();
        arrow.targetObject = order.building.transform;
        arrow.VehicleArrowTransform = vehicleArrowHolders[thisOrderIndex +1];
        arrow.displayImage = arrowImage;
    }

    private void Update()
    {
        if (gameState.currentOrders[gameState.selctedOrderIndex] == thisOrder)
        {
            gameObject.transform.localScale = new Vector3(1, 1.2f, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Destructor(GameState.Order order)
    {
        if (order == thisOrder)
        {
            Destroy(arrow);
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        OnOrderRemove -= Destructor;
    }
}
