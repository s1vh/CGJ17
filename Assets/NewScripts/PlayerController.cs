using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed;
    private float x, y;
    private Rigidbody2D rigid;

    public float distanceToFollow;
    public int numberoffollowers;
	// Use this for initialization
	void Awake () {
        rigid = GetComponent<Rigidbody2D>();
	}

    void Update()  // el movimiento debe ir en el update
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rigid.AddForce(new Vector2(x * speed, y * speed));
    }
}
