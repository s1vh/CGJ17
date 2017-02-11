using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persona : MonoBehaviour {

    public float enmity = 1f;    // enmity is a 0~1 float
    public bool leader = false;

    float speed = 1f;
    Transform persona;
    Rigidbody2D personaRigidbody;

    // Use this for initialization
    void Start()
    {
        personaRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update () {

		if (leader)
        {

        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {

    }

   /*void OnTriggerExit2D(Collider2D other)
    {
        leader = false;
        PersonaRigidbody.MovePosition(this.transform.position);
        MainCharacter.acogidos--;
    }*/
}
