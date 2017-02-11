using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour {

    // Transform MCPosition;
    public float speed = 3f;
    Vector3 movement;
    Rigidbody2D playerRigidbody;
    static public int acogidos = 0; //Esto es una barbaridad, lo se

    // Use this for initialization
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()  // el movimiento debe ir en el update
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Move(h, v);

        //Turning();

       // Animating(h, v);
    }

    void Move(float h, float v)
    {

        movement.Set(h, v, 0f);

        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }
}
