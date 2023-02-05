using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxLife : MonoBehaviour
{
    public GameState gameState;

    private float lifetime;
    private GameState.Order boxOrder;

    // Start is called before the first frame update
    void Start()
    {
        lifetime = 5;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
    }

    public void Spawn(Vector3 pos, Vector3 vel, GameState.Order order)
    {
        boxOrder = order;
        gameObject.SetActive(true);
        gameObject.transform.position = pos;
        gameObject.GetComponent<Rigidbody>().velocity = vel;
        gameObject.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (GameState.Order order in gameState.currentOrders)
        {
            if (collision.gameObject == order.building)
            {
                gameState.Deliver(order);
                Despawn();
                break;
            }
        }
    }
}
