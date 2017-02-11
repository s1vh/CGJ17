using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persona : MonoBehaviour {

    public float velocidad = 1f;

    Transform TransformLider;
    Vector3 PersonaToLider;
    Rigidbody2D PersonaRigidbody;
    bool ConLider = false;
    


    void Awake()
    {

        PersonaRigidbody = GetComponent<Rigidbody2D>();
    }

	void FixedUpdate () {
		if (ConLider)
        {
            PersonaToLider = TransformLider.transform.position - this.transform.position;
            PersonaToLider.z = 0f;
            PersonaRigidbody.AddForce(PersonaToLider * velocidad);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ConLider = true;
            TransformLider = other.transform;
            MainCharacter.acogidos++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ConLider = false;
        PersonaRigidbody.MovePosition(this.transform.position);
        MainCharacter.acogidos--;
    }
}
