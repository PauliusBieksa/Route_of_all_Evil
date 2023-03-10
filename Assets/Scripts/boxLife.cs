using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxLife : MonoBehaviour
{
    public GameState gameState;

    private float lifetime;
    private GameState.Order boxOrder;
    private bool failedOrder = false;

    // Start is called before the first frame update
    void Start()
    {
        lifetime = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < 0.2) lifetime -= Time.deltaTime;
        if (lifetime <= 0 && !failedOrder)
        {
            gameState.FailToDeliver(boxOrder);
            failedOrder = true;
        }
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
        if (failedOrder) return;
        {
            if (collision.gameObject == boxOrder.building)
            {
                gameState.Deliver(boxOrder);
                Despawn();
            }

        }
    }
}
