using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public struct Order
    {
        public float reward;
        public string address;
        public string item;
        public GameObject building;

        public Order(float reward, string address, string item, GameObject building)
        {
            this.reward = reward;
            this.address = address;
            this.item = item;
            this.building = building;
        }
        public Order(Order other)
        {
            this.reward = other.reward;
            this.address = other.address;
            this.item = other.item;
            this.building = other.building;
        }
    }
    // Editor values
    public GameObject roadsContainer;
    public List<boxLife> boxes;
    public int numOfOrders;
    public float startingCash;
    public float cashBleedRate;   
    public int[] pricings = { 60, 150, 300, 420 };
    public GameObject van;
    public float throwForce;
    public GameObject orderLayout;
    public GameObject orderDisplayer;
    public Material originalMaterial;
    public Material tintedMaterial;
    public GameObject warehouse;
    public GameObject pickupZone;

    // lists
    private List<string> itemNames = new List<string>();
    private List<Order> addresses = new List<Order>(); 

    [HideInInspector]
    public List<Order> currentOrders = new List<Order>();

    [HideInInspector]
    public float cash;
    [HideInInspector]
    public int selctedOrderIndex = 0;
    private int nextBoxIndex = 0;
    private Vector3 throwDirection;
    private OrderVisualiser orderVisualiser;

    // Start is called before the first frame update
    void Start()
    {
        itemNames.Add("Priceless vase");
        itemNames.Add("TV");
        itemNames.Add("Antique tea set");
        itemNames.Add("Preserve jars");
        itemNames.Add("Wine");
        itemNames.Add("Laptop");
        itemNames.Add("Clay pots");
        itemNames.Add("Live bees");
        itemNames.Add("Explosives");
        itemNames.Add("Not explosives");
        itemNames.Add("Dumbbells");
        itemNames.Add("Wine glass set");
        itemNames.Add("Bootleg phone");
        itemNames.Add("Bootleg flash drives");

        foreach (Transform road in roadsContainer.transform)
        {
            foreach (Transform buldingsContainer in road)
            {
                if (buldingsContainer.name != "Buildings") continue;
                foreach (Transform buildingNumber in buldingsContainer)
                {
                    string address = road.name + " " + buildingNumber.name;
                    int difficulty = int.Parse(road.gameObject.tag.Substring(road.gameObject.tag.Length - 1));
                    addresses.Add(new Order(pricings[difficulty - 1], address, "", buildingNumber.gameObject));
                }
            }
        }

        cash = startingCash;
        throwDirection = new Vector3(-45, 0, 0);

        //GetOrders();
    }

    void Update()
    {
        cash -= cashBleedRate * Time.deltaTime;
        if (cash < 0) GameOver();
    }


    // Generate new orders
    private List<Order> CreateOrders(int numberOfOrders)
    {
        List<Order> orders = new List<Order>();
        for (int i = 0; i < numberOfOrders; i++)
        {
            Order order = new Order(addresses[Random.Range(0, addresses.Count)]);
            order.item = itemNames[Random.Range(0, itemNames.Count)];
            order.reward += Random.Range(0, 120);
            orders.Add(order);
            GameObject displayer = Instantiate(orderDisplayer, orderLayout.transform);
            displayer.GetComponent<OrderVisualiser>().Initialise(order, i);

            order.building.GetComponent<MeshRenderer>().material = tintedMaterial;
        }
        return orders;
    }

    public void GetOrders()
    {
        currentOrders.Clear();
        currentOrders = CreateOrders(numOfOrders);
        pickupZone.SetActive(false);
    }

    public void Deliver(Order order)
    {
        cash += order.reward;
        order.building.GetComponent<MeshRenderer>().material = originalMaterial;
        RemoveOrder(order);
    }

    public void FailToDeliver(Order order)
    {
        order.building.GetComponent<MeshRenderer>().material = originalMaterial;
        RemoveOrder(order);
    }

    private void RemoveOrder(Order order)
    {
        //public Order(float reward, string address, string item, GameObject building)
        currentOrders.Remove(order);
        if (currentOrders.Count == 0)
        {
            currentOrders.Add(new Order(0, "Back to warehouse", "for more fulfilment", warehouse));
            pickupZone.SetActive(true);
        }
    }

    public void ThrowBox()
    {
        boxes[nextBoxIndex].Spawn(GetThrowPosition(), GetThrowVelocity(), currentOrders[selctedOrderIndex]);
    }

    public Vector3 GetThrowPosition()
    {
        return van.transform.position + new Vector3(2.3f, 1.1f, 2.4f);
    }

    public Vector3 GetThrowVelocity()
    {
        return throwDirection * throwForce + van.GetComponent<Rigidbody>().velocity;
    }

    private void GameOver()
    {
        //
    }
}
