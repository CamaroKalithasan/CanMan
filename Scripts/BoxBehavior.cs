using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehavior : MonoBehaviour
{
    public GameObject carrier = null;
    public bool canPickup = true;
    public float cooldown = 4.0f;
    public float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
            GetComponent<Rigidbody2D>().gravityScale = 1;

        if(carrier)
        {
            transform.position =
                new Vector3(carrier.transform.position.x, carrier.transform.position.y + 1.5f, 0);

            if (Input.GetKeyDown(KeyCode.E))
            {
                carrier = null;
                timer = cooldown;
                GetComponent<Rigidbody2D>().gravityScale = 0.1f;
            }
        }

        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
            
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player" && timer <= 0f)
        {
            carrier = collider.gameObject;
        }
    }
}
