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
    public RawImage arrowImage;
    

    private DirectionVisualiser arrow;

    public void Initialise(GameState.Order order, int orderNum)
    {
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
}
